using System;
using System.Collections.Generic;
using Cqrs02.Test.Infrastructure;
using Cqrs03.Test.Infrastructure;

namespace Cqrs01.Test.Infrastructure
{
    public class EntityStorage
    {
        public EntityStorage(Bus bus)
        {
            _bus = bus;
        }
        private readonly Dictionary<Guid, IAggregateEntity> _storage = new Dictionary<Guid, IAggregateEntity>();
        private readonly Bus _bus;

        public void Save<T>(Guid id, AggregateRoot<T> aggregate,int expectedVersion = -1) where T:IAggregateEntity
        {
            if (!_storage.ContainsKey(id))
            {
                _storage[id] = aggregate.Entity;
            }
            else if(_storage[id].Version != expectedVersion)
            {
                throw new Exception("Optimistic Lock Exception");
            }
            else
            {
                _storage[id] = aggregate.Entity;
                _storage[id].Version++;
            }
            foreach(var @event in aggregate.GetUnsentEvents())
            {
                _bus.Send(@event);
            }

        }

        public T GetById<T>(Guid id) where T: IAggregateEntity
        {
            if (_storage.ContainsKey(id) && _storage[id].GetType().IsAssignableFrom(typeof(T)))
            {
                return (T)_storage[id];
            }
            return default(T);
        }
    }
}
