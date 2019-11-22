using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace InMemory.Bus
{
    public class SecondMessage
    {
        public SecondMessage()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
    }
}
