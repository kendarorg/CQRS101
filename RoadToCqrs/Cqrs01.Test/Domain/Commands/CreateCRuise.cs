using System;

namespace Cqrs01.Test.Domain.Commands
{
    public class CreateCruise
    {
        public Guid CruiseId { get;  set; }
        public string Name { get;  set; }
    }
}
