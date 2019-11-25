
using System;
using System.Collections.Generic;

namespace SimplestPossibleThing.Lib.Projection
{
    public interface IInventoryItemListRepository
    {
        void Save(InventoryItemListDto inventoryItemListDto);
        InventoryItemListDto GetById(Guid id);
        void Delete(Guid id);
        IEnumerable<InventoryItemListDto> GetAll();
    }
}
