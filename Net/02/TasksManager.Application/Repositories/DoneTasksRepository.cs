using Cqrs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace TasksManager.Repositories
{
    public class DoneTasksRepository : IRepository<DoneTaskDao, Guid>
    {
        private static ConcurrentDictionary<Guid, DoneTaskDao> _storage = new ConcurrentDictionary<Guid, DoneTaskDao>();

        public void Delete(Guid id)
        {
            DoneTaskDao deleted;
            if (_storage.ContainsKey(id)) _storage.TryRemove(id, out deleted);
        }

        public List<DoneTaskDao> GetAll()
        {
            return _storage.Values.ToList();
        }

        public DoneTaskDao GetById(Guid id)
        {
            if (_storage.ContainsKey(id)) return _storage[id];
            return null;
        }

        public DoneTaskDao Save(DoneTaskDao item)
        {
            _storage[item.Id] = item;
            return item;
        }
    }
}
