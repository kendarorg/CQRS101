using System;
using System.Collections.Generic;
using Cqrs02.Test.Infrastructure;

namespace Cqrs01.Test.Infrastructure
{
    public class EntityStorage
    {
        public EntityStorage(Bus bus)
        {
            _bus = bus;
        }
        private readonly Dictionary<Guid, object> _storage = new Dictionary<Guid, object>();
        private readonly Bus _bus;

        public void Save<T>(Guid id, AggregateRoot<T> aggregate)
        {
            _storage[id] = aggregate.Entity;
            foreach(var @event in aggregate.GetUnsentEvents())
            {
                _bus.Send(@event);
            }

        }

        public T GetById<T>(Guid id)
        {
            return (T)_storage[id];
        }
    }
}
