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
            channel.QueueDeclare(queueName, true);
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
                    Console.WriteLine(ex);
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
