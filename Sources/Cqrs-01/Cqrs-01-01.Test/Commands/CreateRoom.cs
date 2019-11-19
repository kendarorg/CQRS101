using System;

namespace Cruise.Commands
{
    public class CreateRoom
    {
        public CreateRoom(Guid cruiseId, int number, int beds, int @class)
        {
            CruiseId = cruiseId;
            Number = number;
            Beds = beds;
            Class = @class;
        }
        public Guid CruiseId{ get; }
        public int Number{ get; }
        public int Beds{ get; }
        public int Class{ get; }
    }
}
