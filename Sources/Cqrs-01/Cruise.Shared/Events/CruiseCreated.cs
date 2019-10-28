using System;

namespace Cruise.Events
{
    public class CruiseCreated
    {
        public CruiseCreated(string name, Guid cruiseId)
        {
            Name = name;
            CruiseId = cruiseId;
        }
        public string Name{ get; }
        public Guid CruiseId{ get; }
    }
}
