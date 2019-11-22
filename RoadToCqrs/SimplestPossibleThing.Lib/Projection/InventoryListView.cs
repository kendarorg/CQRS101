using SimplestPossibleThing.Lib.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplestPossibleThing.Lib.Projection
{
    public class InventoryListView
    {
        private readonly IInventoryItemListRepository _repository;

        public InventoryListView(IInventoryItemListRepository repository)
        {
            _repository = repository;
        }
        public void Handle(InventoryItemCreated message)
        {
            _repository.Save(new InventoryItemListDto(message.Id, message.Name));
        }

        public void Handle(InventoryItemRenamed message)
        {
            var item = _repository.GetById( message.Id);
            item.Name = message.NewName;
            _repository.Save(item);
        }

        public void Handle(InventoryItemDeactivated message)
        {
            _repository.Delete(message.Id);
        }
    }
}
