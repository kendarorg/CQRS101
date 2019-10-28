using Crud;
using System;

namespace Cruise
{
    public class RoomsForTripsEntity : IEntity
    {
        public Guid Id { get; set; }
        public int Class { get; set; }
        public Guid CruiseId { get; set; }
        public int Count { get; set; }
    }
}
