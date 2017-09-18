using System;
using Utils;

namespace Cqrs
{
    public interface IBus: ISingleton
    {
        void RegisterHandler(Action<object> handlerFunc,Type messageType);
        void Send(IMessage message);
    }
}
