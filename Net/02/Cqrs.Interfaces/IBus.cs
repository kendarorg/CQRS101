using System;
using System.Collections.Generic;
using Utils;

namespace Cqrs
{
    public interface IBus : ISingleton
    {
        Type GetType(String name);
        IEnumerable<String> GetTypes();
        void RegisterHandler(Action<object> handlerFunc, Type messageType);
        void Send(IMessage message);
    }
}
