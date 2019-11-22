using Infrastructure.Lib.Cqrs;
using System;

namespace SimplestPossibleThing.Lib.Commands
{
    public class CreateInventoryItem : ICommand
    {
        public Guid InventoryItemId { get; set; }
        public string Name { get; set; }

        public CreateInventoryItem(Guid inventoryItemId, string name)
        {
            InventoryItemId = inventoryItemId;
            Name = name;
        }
    }
}
