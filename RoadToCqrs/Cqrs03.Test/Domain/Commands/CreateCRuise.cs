using System;

namespace Cqrs01.Test.Domain.Commands
{
    public class CreateCruise
    {
        public CreateCruise(Guid cruiseId,string name)
        {
            CruiseId = cruiseId;
            Name = name;
        }
        public Guid CruiseId { get;  set; }
        public string Name { get;  set; }
    }
}
