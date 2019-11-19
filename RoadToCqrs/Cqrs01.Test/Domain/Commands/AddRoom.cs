using System;

namespace Cqrs01.Test.Domain.Commands
{
    public class AddRoom
    {
        public Guid CruiseId { get; set; }
        public int Number { get; set; }
        public int Category { get; set; }
    }
}
