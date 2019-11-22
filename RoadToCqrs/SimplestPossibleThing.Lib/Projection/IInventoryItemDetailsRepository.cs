using System;

namespace SimplestPossibleThing.Lib.Projection
{
    public interface IInventoryItemDetailsRepository
    {
        void Save(InventoryItemDetailsDto inventoryItemDetailsDto);
        InventoryItemDetailsDto GetById(Guid id);
    }
}
