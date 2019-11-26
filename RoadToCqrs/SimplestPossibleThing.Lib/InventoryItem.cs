using Infrastructure.Lib.Cqrs;
using SimplestPossibleThing.Lib.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplestPossibleThing.Lib
{
    public class InventoryItem : AggregateRoot<InventoryItemEntity>
    {
        public InventoryItem(InventoryItemEntity entity) : base(entity)
        {
        }

        public InventoryItem(Guid id, string name)
        {
            Entity = new InventoryItemEntity(id, name);
            Entity.Version = -1;
            Publish(new InventoryItemCreated(id, name));
        }

        
        public void ChangeName(string newName)
        {
            if (string.IsNullOrEmpty(newName)) throw new ArgumentException("newName");
            Entity.Name = newName;
            Publish(new InventoryItemRenamed(Entity.Id, newName));
        }

        public void Remove(int count)
        {
            if (count <= 0) throw new InvalidOperationException("cant remove negative count from inventory");
            Entity.Items -= count;
            Publish(new ItemsRemovedFromInventory(Entity.Id, count));
        }


        public void CheckIn(int count)
        {
            if (count <= 0) throw new InvalidOperationException("must have a count greater than 0 to add to inventory");
            Entity.Items += count;
            Publish(new ItemsRemovedFromInventory(Entity.Id, count));
        }

        public void Deactivate()
        {
            if (!Entity.Deactivated) throw new InvalidOperationException("already deactivated");
            Entity.Deactivated = true;
            Publish(new InventoryItemDeactivated(Entity.Id));
        }
    }
}
