using Bus;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtils
{
    public class BaseBusTest
    {
        protected List<object> _messages;
        protected Mock<IBus> _bus;

        protected virtual void Initialize()
        {
            _messages = new List<object>();
            _bus = new Mock<IBus>();
            _bus.Setup(bus => bus.Send(It.IsAny<object>(),It.IsAny<DateTime?>()))
                .Callback((object message,DateTime? expiration) =>
                {
                    _messages.Add(message);
                });
        }
    }
}
