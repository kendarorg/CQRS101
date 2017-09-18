using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cqrs
{
    public class Bus : IBus
    {
        public Bus(IList<IMessageHandler> validators)
        {
            for (int i = 0; i < validators.Count(); i++)
            {
                var validator = validators[i];
                validator.Register(this);

            }
        }

        private Dictionary<Type, List<Action<object>>> _handlerFunctions = new Dictionary<Type, List<Action<object>>>();

        /// <summary>
        /// This method contains the system to retrieve the handle methods from all
        /// message handlers passed
        /// </summary>
        public void RegisterHandler(Action<object> handlerFunction, Type messageType)
        {
            if (!_handlerFunctions.ContainsKey(messageType))
            {
                _handlerFunctions.Add(messageType, new List<Action<object>>());
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
                        Debug.WriteLine(ex);
                    }
                }
            }
        }
    }
}
