
using Infrastructure.Lib.ServiceBus;
using SimplestPossibleThing.Lib.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplestPossibleThing.Lib.Projection
{
    public class InventoryListView 
    {
        private readonly IInventoryItemListRepository _repository;

        public InventoryListView(IBus bus,IInventoryItemListRepository repository)
        {
            _repository = repository;

            bus.Register<InventoryItemCreated>(Handle, "InventoryListView");
            bus.Register<InventoryItemRenamed>(Handle, "InventoryListView");
            bus.Register<InventoryItemDeactivated>(Handle, "InventoryListView");
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
