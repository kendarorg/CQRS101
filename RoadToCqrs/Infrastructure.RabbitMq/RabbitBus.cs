using Infrastructure.Lib.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Rabbit
{
    public class RabbitBus : IBus
    {
        public void Send(object message)
        {
            throw new NotImplementedException();
        }
    }
}
