using Infrastructure.Lib.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplestPossibleThing.Lib.Events
{
    public class InventoryItemDeactivated : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }

        public InventoryItemDeactivated(Guid id)
        {
            Id = id;
        }
    }
}
