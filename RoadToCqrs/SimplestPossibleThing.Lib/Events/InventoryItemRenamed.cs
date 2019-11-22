using Infrastructure.Lib.Cqrs;
using System;

namespace SimplestPossibleThing.Lib.Events
{
    public class InventoryItemRenamed : IEvent
    {
        public Guid Id { get; set; }
        public string NewName { get; set; }
        public int Version { get; set; }

        public InventoryItemRenamed(Guid id, string newName)
        {
            Id = id;
            NewName = newName;
        }
    }
}
