
using Infrastructure.Lib.ServiceBus;
using SimplestPossibleThing.Lib.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplestPossibleThing.Lib.Projection
{
    public class InventoryItemDetailView
    {
        private readonly IInventoryItemDetailsRepository _repository;

        public InventoryItemDetailView(IBus bus,IInventoryItemDetailsRepository repository)
        {
            _repository = repository;
            bus.Register<InventoryItemCreated>(Handle);
            bus.Register<InventoryItemRenamed>(Handle);
            bus.Register<InventoryItemDeactivated>(Handle);
            bus.Register<ItemsCheckedInToInventory>(Handle);
            bus.Register<ItemsRemovedFromInventory>(Handle);
        }
        public void Handle(InventoryItemCreated message)
        {
            _repository.Save( new InventoryItemDetailsDto(message.Id, message.Name, 0, 0));
        }

        public void Handle(InventoryItemRenamed message)
        {
            InventoryItemDetailsDto d = GetDetailsItem(message.Id);
            d.Name = message.NewName;
            d.Version = message.Version;
            _repository.Save(d);
        }

        private InventoryItemDetailsDto GetDetailsItem(Guid id)
        {
            InventoryItemDetailsDto d = _repository.GetById(id);

            if (null == d)
            {
                throw new InvalidOperationException("did not find the original inventory this shouldnt happen");
            }

            return d;
        }

        public void Handle(ItemsRemovedFromInventory message)
        {
            InventoryItemDetailsDto d = GetDetailsItem(message.Id);
            d.CurrentCount -= message.Count;
            d.Version = message.Version;
            _repository.Save(d);
        }

        public void Handle(ItemsCheckedInToInventory message)
        {
            InventoryItemDetailsDto d = GetDetailsItem(message.Id);
            d.CurrentCount += message.Count;
            d.Version = message.Version;
            _repository.Save(d);
        }

        public void Handle(InventoryItemDeactivated message)
        {
            _repository.Delete(message.Id);
        }
    }
}
