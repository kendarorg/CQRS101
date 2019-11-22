using Infrastructure.Lib.Cqrs;
using SimplestPossibleThing.Lib.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplestPossibleThing.Lib
{
    public class InventoryCommandHandlers
    {
        private readonly IEntityStorage _repository;

        public InventoryCommandHandlers(IEntityStorage repository)
        {
            _repository = repository;
        }

        public void Handle(CreateInventoryItem message)
        {
            var item = new InventoryItem(message.InventoryItemId, message.Name);
            _repository.Save(item, -1);
        }

        public void Handle(DeactivateInventoryItem message)
        {
            var item = _repository.GetById<InventoryItem>(message.InventoryItemId);
            item.Deactivate();
            _repository.Save(item, message.OriginalVersion);
        }

        public void Handle(RemoveItemsFromInventory message)
        {
            var item = _repository.GetById<InventoryItem>(message.InventoryItemId);
            item.Remove(message.Count);
            _repository.Save(item, message.OriginalVersion);
        }

        public void Handle(CheckInItemsToInventory message)
        {
            var item = _repository.GetById<InventoryItem>(message.InventoryItemId);
            item.CheckIn(message.Count);
            _repository.Save(item, message.OriginalVersion);
        }

        public void Handle(RenameInventoryItem message)
        {
            var item = _repository.GetById<InventoryItem>(message.InventoryItemId);
            item.ChangeName(message.NewName);
            _repository.Save(item, message.OriginalVersion);
        }
    }
}
