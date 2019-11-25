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
        private readonly IAdvancedBus _advancedBus;


        public RabbitBus(string rabbitHost = "localhost")
        {
            _rabbitHost = rabbitHost;

            _advancedBus = RabbitHutch.CreateBus("host=" + rabbitHost).Advanced;
            _advancedBus.ExchangeDeclare("events", ExchangeType.Topic);
            _advancedBus.ExchangeDeclare("commands", ExchangeType.Direct);
        }



        public void Register<T>(Action<T> handler) where T:class
        {
            string exchangeName = "commands";
            var exchangeType = ExchangeType.Direct;
            if (typeof(IEvent).IsAssignableFrom(typeof(T)))
            {
                exchangeType = ExchangeType.Topic;
                exchangeName = "events";
            }
            var queue = _advancedBus.QueueDeclare(typeof(T).Name);
            var exchange = _advancedBus.ExchangeDeclare(exchangeName, exchangeType);
            var binding = _advancedBus.Bind(exchange, queue, typeof(T).Name);
            _advancedBus.Consume(queue, x => x
                    .Add<T>((message, info) =>
                    {
                        handler(message.Body);
                    })
                    .ThrowOnNoMatchingHandler = false
                );
        }

        public void Send(object message, TimeSpan? delay = null)
        {
            if (typeof(IEvent).IsAssignableFrom(message.GetType()))
            {
                var exchange = _advancedBus.ExchangeDeclare("events", ExchangeType.Topic);
                _advancedBus.Publish(exchange, message.GetType().Name,true,new MessageProperties
                {
                    
                }, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
            }
            else if (typeof(ICommand).IsAssignableFrom(message.GetType()))
            {
                var exchange = _advancedBus.ExchangeDeclare("commands", ExchangeType.Direct);
                _advancedBus.Publish(exchange, message.GetType().Name, true, new MessageProperties
                {

                }, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
            }
        }
    }
}
