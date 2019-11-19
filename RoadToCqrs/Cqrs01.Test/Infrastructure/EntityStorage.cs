using System;
using System.Collections.Generic;

namespace Cqrs01.Test.Infrastructure
{
    public class EntityStorage
    {
        public EntityStorage()
        {
            Events = new List<object>();
        }
        private readonly Dictionary<Guid, object> _storage = new Dictionary<Guid, object>();
        public List<object> Events { get; private set; }
        public void Save<T>(Guid id, AggregateRoot<T> aggregate)
        {
            Events.Clear();
            _storage[id] = aggregate.Entity;
            Events.AddRange(aggregate.GetUnsentEvents());

        }

        public T GetById<T>(Guid id)
        {
            return (T)_storage[id];
        }
    }
}
