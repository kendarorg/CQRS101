using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Es
{
    public class AggregateRoot
    {
        private static ConcurrentDictionary<Type, ConcurrentDictionary<Type, Action<AggregateRoot, object>>> _handlerFunctions =
            new ConcurrentDictionary<Type, ConcurrentDictionary<Type, Action<AggregateRoot, object>>>();

        private static HashSet<Type> _aggregateRegistered = new HashSet<Type>();

        protected static bool IsRegistered(AggregateRoot instance)
        {
            var instanceType = instance.GetType();
            return _aggregateRegistered.Contains(instanceType);
        }

        protected static void Register<T>(AggregateRoot instance, Action<AggregateRoot, object> handlerFunction)
        {
            var instanceType = instance.GetType();
            if (_aggregateRegistered.Contains(instanceType)) return;

            var messageType = typeof(T);

            if (!_handlerFunctions.ContainsKey(instanceType))
            {
                _handlerFunctions[instanceType] = new ConcurrentDictionary<Type, Action<AggregateRoot, object>>();
            }
            _handlerFunctions[instanceType][messageType] = handlerFunction;
        }
    }
}
