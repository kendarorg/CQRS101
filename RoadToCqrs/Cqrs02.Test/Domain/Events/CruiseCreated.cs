using System;

namespace Cqrs01.Test.Domain.Events
{
    public class CruiseCreated
    {
        public Guid CruiseId { get; set; }
        public string Name { get; set; }
    }
}
