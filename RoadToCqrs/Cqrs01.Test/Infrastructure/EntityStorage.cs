using System;
using System.Collections.Generic;

namespace Cqrs01.Test.Infrastructure
{
    public class EntityStorage
    {
    	private readonly Dictionary<Guid, object> _storage = new Dictionary<Guid, object>();
        public List<object> Events { get; private set; }
        
        public EntityStorage()
        {
            Events = new List<object>();
        }
        
        public void Save<T>(Guid id, AggregateRoot<T> aggregate)
        {
            Events.Clear();
            _storage[id] = aggregate.Entity;
            foreach(var @event in aggregate.GetUnsentEvents())
            {
                Events.Add(@event);
            }

        }

        public T GetById<T>(Guid id)
        {
            return (T)_storage[id];
        }
    }
}
