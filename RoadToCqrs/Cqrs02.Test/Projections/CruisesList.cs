using System;

namespace Cqrs02.Test.Projections
{
    public class CruisesListEntity
    {
        public Guid CruiseId { get; set; }
        public string Name { get; set; }
        public int FirstClassRooms { get; set; }
        public int SecondClassRooms { get; set; }
        public int ThirdClassRooms { get; set; }

        public CruisesListEntity(Guid cruiseId, string name)
        {
            CruiseId = cruiseId;
            Name = name;
        }
    }
}
