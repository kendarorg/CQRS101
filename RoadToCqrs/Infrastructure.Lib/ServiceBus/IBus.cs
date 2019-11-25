
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Lib.ServiceBus
{
    public interface IBus
    {
        void Send(object message, TimeSpan? delay = null);
        void Register<T>(Action<T> handler) where T : class;
    }
}
