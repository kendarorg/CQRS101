using Infrastructure.Lib.Cqrs;
using System;

namespace SimplestPossibleThing.Lib.Events
{
    public class ItemsCheckedInToInventory : IEvent
    {
        public Guid Id { get; set; }
        public int Count { get; set; }
        public int Version { get; set; }

        public ItemsCheckedInToInventory(Guid id, int count)
        {
            Id = id;
            Count = count;
        }
    }
}
