using Crud;
using System;
using System.Collections.Generic;

// ReSharper disable MemberCanBePrivate.Global

namespace Cruise
{
    public class CruiseEntity : IOptimisticEntity
    {
        public CruiseEntity(Guid id, long version, string name, List<Room> rooms)
        {
            Id = id;
            Version = version;
            Name = name;
            Rooms = rooms;
        }
        public Guid Id { get; set; }
        public long Version { get; set; }
        public string Name { get; set; }
        public List<Room> Rooms { get; set; }
    }

    public class Room
    {
        public Room(Guid cruiseId, int number, int @class, int beds, Guid roomId)
        {
            CruiseId = cruiseId;
            Number = number;
            Class = @class;
            Beds = beds;
            RoomId = roomId;
        }
        public Guid CruiseId { get; set; }
        public int Number { get; set; }
        public int Class { get; set; }
        public int Beds { get; set; }
        public Guid RoomId { get; set; }
    }
}
