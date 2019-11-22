using Infrastructure.Lib.Cqrs;
using System;

namespace SimplestPossibleThing.Lib.Commands
{
    public class CheckInItemsToInventory:ICommand
    {
        public Guid InventoryItemId {get;set;}
        public int Count { get; set; }
        public int OriginalVersion { get; set; }

        public CheckInItemsToInventory(Guid inventoryItemId, int count, int originalVersion)
        {
            InventoryItemId = inventoryItemId;
            Count = count;
            OriginalVersion = originalVersion;
        }
    }
}
