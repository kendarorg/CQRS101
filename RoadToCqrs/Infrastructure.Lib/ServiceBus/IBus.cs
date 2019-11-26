
using Infrastructure.Lib.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Lib.ServiceBus
{
    public interface IBus
    {
        void Send(object message,TimeSpan? delay=null);
        void Register<T>(Action<T> handler, string prefix = "") where T : class;
    }
}
