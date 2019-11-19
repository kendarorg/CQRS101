using Cqrs03.Test.Infrastructure;
using System;
using System.Collections.Generic;

namespace Cqrs01.Test.Domain
{
    public class CruiseEntity: IAggregateEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Room> Rooms { get; set; }
        public int Version { get; set; }

        public CruiseEntity()
        {
            Rooms = new List<Room>();
        }
        public CruiseEntity(Guid id, string name)
        {
            Id = id;
            Name = name;
            Rooms = new List<Room>();
        }
    }

    public class Room
    {
        public int Number { get; set; }
        public int Category { get; set; }
    }
}
