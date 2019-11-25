using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Lib.ServiceBus
{
    public interface IMessageHandler
    {

    }
    public interface IMessageHandler<T> : IMessageHandler
    {
        void Handle(T message);
    }
}
