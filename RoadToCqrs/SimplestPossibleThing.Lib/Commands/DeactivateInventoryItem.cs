using Infrastructure.Lib.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplestPossibleThing.Lib.Commands
{
    public class DeactivateInventoryItem : ICommand
    {
        public Guid InventoryItemId { get; set; }
        public int OriginalVersion { get; set; }

        public DeactivateInventoryItem(Guid inventoryItemId, int originalVersion)
        {
            InventoryItemId = inventoryItemId;
            OriginalVersion = originalVersion;
        }
    }
}
