using SimplestPossibleThing.Lib.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplestPossibleThing.Lib.Projection
{
    public class InventoryItemDetailView
    {
        private readonly IInventoryItemDetailsRepository _repository;

        public InventoryItemDetailView(IInventoryItemDetailsRepository repository)
        {
            _repository = repository;
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
        }

        public void Handle(ItemsCheckedInToInventory message)
        {
            InventoryItemDetailsDto d = GetDetailsItem(message.Id);
            d.CurrentCount += message.Count;
            d.Version = message.Version;
        }

        public void Handle(InventoryItemDeactivated message)
        {
            BullShitDatabase.details.Remove(message.Id);
        }
    }
}
