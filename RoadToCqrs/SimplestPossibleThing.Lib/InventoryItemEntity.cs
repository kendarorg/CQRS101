using Infrastructure.Lib.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplestPossibleThing.Lib
{
    public class InventoryItemEntity : IAggregateEntity
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public int Items { get; set; }
        public bool Deactivated { get; set; }

        public InventoryItemEntity(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
