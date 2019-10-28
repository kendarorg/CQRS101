using NServiceBus;
using System;

namespace Cruise.Commands
{
    public class CreateRoom:ICommand
    {
        public CreateRoom(Guid cruiseId, int number, int beds, int @class)
        {
            CruiseId = cruiseId;
            Number = number;
            Beds = beds;
            Class = @class;
        }
        public Guid CruiseId{ get; set; }
        public int Number{ get; set; }
        public int Beds{ get; set; }
        public int Class{ get; set; }
    }
}
