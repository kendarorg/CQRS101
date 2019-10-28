using NServiceBus;
using System;

namespace Cruise.Events
{
    public class CruiseCreated:IEvent
    {
        public CruiseCreated(string name, Guid cruiseId)
        {
            Name = name;
            CruiseId = cruiseId;
        }
        public string Name{ get; set; }
        public Guid CruiseId{ get; set; }
    }
}
