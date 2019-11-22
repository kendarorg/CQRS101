using Infrastructure.Lib.Cqrs;
using System;

namespace SimplestPossibleThing.Lib.Projection
{
    public class InventoryItemListDto:IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public InventoryItemListDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
