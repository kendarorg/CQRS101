using Infrastructure.Lib.Cqrs;
using System;

namespace SimplestPossibleThing.Lib.Events
{
    public class InventoryItemCreated:IEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public InventoryItemCreated(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
