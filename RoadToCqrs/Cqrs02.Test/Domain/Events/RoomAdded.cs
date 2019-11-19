using System;

namespace Cqrs01.Test.Domain.Events
{
    public class RoomAdded
    {
        public Guid CruiseId { get; set; }
        public int Number { get; set; }
        public int Category { get; set; }
    }
}
