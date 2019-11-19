using System;

namespace Cruise.Events
{
    public class RoomCreated
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
        public Guid CruiseId{ get; }
        public int Number{ get; }
        public int Class{ get; }
        public int Beds{ get; }
        public bool Success{ get; set; }
        public Guid RoomId{ get; }
    }
}
