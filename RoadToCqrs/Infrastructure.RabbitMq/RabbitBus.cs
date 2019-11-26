using EasyNetQ;
using Infrastructure.Lib.Cqrs;
using Infrastructure.Lib.ServiceBus;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Infrastructure.Rabbit
{
    public class RabbitBus : Lib.ServiceBus.IBus
    {
        private readonly string _rabbitHost;
        private readonly IConnection _connection;

        public RabbitBus(string rabbitHost = "localhost")
        {
            _rabbitHost = rabbitHost;
            var factory = new ConnectionFactory() { HostName = rabbitHost };
            _connection = factory.CreateConnection();
            
            var channel = _connection.CreateModel();
            channel.ExchangeDeclare("commands.dl.exchange", ExchangeType.Fanout);
            channel.QueueDeclare
                (
                    "commands.dl", true, false, false,
                    new Dictionary<string, object> {
                        { "x-dead-letter-exchange", "commands" },
                        { "x-message-ttl", 30000 },
                    }
                );
            channel.QueueBind("commands.dl", "commands.dl.exchange", string.Empty, null);
        }

		private static IDictionary<string, object> CopyHeaders(IBasicProperties originalProperties)
		{
		    IDictionary<string, object> dict = new Dictionary<string, object>();
		    IDictionary<string, object> headers = originalProperties.Headers;
		    if (headers != null)
		    {
		        foreach (KeyValuePair<string, object> kvp in headers)
		        {
		            dict[kvp.Key] = kvp.Value;
		        }
		    }
		 
		    return dict;
		}
		 
		private static int GetRetryCount(IBasicProperties messageProperties, string countHeader)
		{
		    IDictionary<string, object> headers = messageProperties.Headers;
		    int count = 0;
		    if (headers != null)
		    {
		        if (headers.ContainsKey(countHeader))
		        {
		            string countAsString = Convert.ToString( headers[countHeader]);
		            count = Convert.ToInt32(countAsString);
		        }
		    }
		 
		    return count;
		}

        public void Register<T>(Action<T> handler, string prefix = "") where T : class
        {
            string queueName = typeof(T).Name;
            string routingKey = typeof(T).Name;
            string exchangeName = "commands";
            var exchangeType = ExchangeType.Direct;
            if (typeof(IEvent).IsAssignableFrom(typeof(T)))
            {
                exchangeName = "events." + queueName;
                queueName = queueName + "." + prefix;
                exchangeType = ExchangeType.Fanout;
                routingKey = "";

            }
            var channel = _connection.CreateModel();
            channel.QueueDeclare(queueName, true, false, false, new Dictionary<string, object>
                    {
                        {"x-dead-letter-exchange", "commands.dl.exchange"},
                        {"x-dead-letter-routing-key", "commands.dl"}
                    });
            channel.ExchangeDeclare(exchangeName, exchangeType, true);
            channel.QueueBind(queueName, exchangeName, routingKey);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var message = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));
                    handler(message);
                }
                catch (Exception ex)
                {
                    channel.BasicReject(ea.DeliveryTag, false);
                }
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public void Send(object message, TimeSpan? delay)
        {
            string queueName = message.GetType().Name;
            string routingKey = queueName;
            string exchangeName = "commands";
            if (typeof(IEvent).IsAssignableFrom(message.GetType()))
            {
                exchangeName = "events." + queueName;
                routingKey = "";
            }
            var channel = _connection.CreateModel();

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            channel.BasicPublish(exchange: exchangeName,
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);
        }
    }
}


private static void ReceiveBadMessageExtended(IModel model)
{
    model.BasicQos(0, 1, false);
    QueueingBasicConsumer consumer = new QueueingBasicConsumer(model);
    model.BasicConsume(RabbitMqService.BadMessageBufferedQueue, false, consumer);
    string customRetryHeaderName = "number-of-retries";
    int maxNumberOfRetries = 3;
    while (true)
    {
        BasicDeliverEventArgs deliveryArguments = consumer.Queue.Dequeue() as BasicDeliverEventArgs;
        String message = Encoding.UTF8.GetString(deliveryArguments.Body);
        Console.WriteLine("Message from queue: {0}", message);
        Random random = new Random();
        int i = random.Next(0, 3);
        int retryCount = GetRetryCount(deliveryArguments.BasicProperties, customRetryHeaderName);
        if (i == 2) //no exception, accept message
        {
            Console.WriteLine("Message {0} accepted. Number of retries: {1}", message, retryCount);
            model.BasicAck(deliveryArguments.DeliveryTag, false);
        }
        else //simulate exception: accept message, but create copy and throw back
        {
            if (retryCount < maxNumberOfRetries)
            {
                Console.WriteLine("Message {0} has thrown an exception. Current number of retries: {1}", message, retryCount);
                IBasicProperties propertiesForCopy = model.CreateBasicProperties();
                IDictionary<string, object> headersCopy = CopyHeaders(deliveryArguments.BasicProperties);
                propertiesForCopy.Headers = headersCopy;
                propertiesForCopy.Headers[customRetryHeaderName] = ++retryCount;
                model.BasicPublish(deliveryArguments.Exchange, deliveryArguments.RoutingKey, propertiesForCopy, deliveryArguments.Body);
                model.BasicAck(deliveryArguments.DeliveryTag, false);
                Console.WriteLine("Message {0} thrown back at queue for retry. New retry count: {1}", message, retryCount);
            }
            else //must be rejected, cannot process
            {
                Console.WriteLine("Message {0} has reached the max number of retries. It will be rejected.", message);
                model.BasicReject(deliveryArguments.DeliveryTag, false);
            }
        }
    }
}