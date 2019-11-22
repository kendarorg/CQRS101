using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Lib.Cqrs
{
    public interface IBus
    {
        void Send(object message);
    }
}
