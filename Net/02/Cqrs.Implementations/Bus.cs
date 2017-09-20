using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cqrs
{
    public class Bus : IBus
    {
        private static ILog _logger = LogManager.GetLogger(typeof(Bus));

        public Bus(IList<IMessageHandler> validators)
        {
            for (int i = 0; i < validators.Count(); i++)
            {
                var validator = validators[i];
                validator.Register(this);

            }
        }

        private ConcurrentDictionary<String, Type> _messageTypes = new ConcurrentDictionary<string, Type>();
        private ConcurrentDictionary<Type, List<Action<object>>> _handlerFunctions = new ConcurrentDictionary<Type, List<Action<object>>>();

        /// <summary>
        /// This method contains the system to retrieve the handle methods from all
        /// message handlers passed
        /// </summary>
        public void RegisterHandler(Action<object> handlerFunction, Type messageType)
        {
            if (!_handlerFunctions.ContainsKey(messageType))
            {
                _handlerFunctions[messageType] = new List<Action<object>>();
            }
            String messageTypeName = messageType.Name.ToUpperInvariant();
            if (!_messageTypes.ContainsKey(messageTypeName))
            {
                _messageTypes[messageTypeName] = messageType;
            }
            _handlerFunctions[messageType].Add(handlerFunction);
        }

        public void Send(IMessage message)
        {
            var messageType = message.GetType();
            if (_handlerFunctions.ContainsKey(messageType))
            {
                var handlerFunction = _handlerFunctions[messageType];
                for (var i = 0; i < handlerFunction.Count; i++)
                {
                    try
                    {
                        handlerFunction[i](message);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Error handling message: " + messageType.Name,ex);
                    }
                }
            }
        }

        public Type GetType(string messageTypeName)
        {
            messageTypeName = messageTypeName.ToUpperInvariant();
            if (!_messageTypes.ContainsKey(messageTypeName))
            {
                return null;
            }
            return _messageTypes[messageTypeName];
        }

        public IEnumerable<string> GetTypes()
        {
            return _messageTypes.Keys;
        }
    }
}
