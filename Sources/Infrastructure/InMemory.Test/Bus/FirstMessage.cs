using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace InMemory.Bus
{
    public class FirstMessage
    {
        public FirstMessage()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
    }
}
