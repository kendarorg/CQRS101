using NServiceBus;
using System;

namespace Cruise.Events
{
    public class RoomCreated:IEvent
    {
        public RoomCreated(Guid cruiseId, int number, int @class, int beds, bool success, Guid roomId)
        {
            CruiseId = cruiseId;
            Number = number;
            Class = @class;
            Beds = beds;
            Success = success;
            RoomId = roomId;
        }
        public Guid CruiseId{ get; set; }
        public int Number{ get; set; }
        public int Class{ get; set; }
        public int Beds{ get; set; }
        public bool Success{ get; set; }
        public Guid RoomId{ get; set; }
    }
}
