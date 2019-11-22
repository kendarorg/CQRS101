using Infrastructure.Lib.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplestPossibleThing.Lib.Projection
{
    public class InventoryItemDetailsDto:IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int CurrentCount { get; set; }
        public int Version { get; set; }

        public InventoryItemDetailsDto(Guid id, string name, int currentCount, int version)
        {
            Id = id;
            Name = name;
            CurrentCount = currentCount;
            Version = version;
        }
    }
}
