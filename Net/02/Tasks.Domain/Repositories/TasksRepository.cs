using Cqrs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Tasks.Repositories
{
    public class TasksRepository : IRepository<TaskDao, Guid>
    {
        private static ConcurrentDictionary<Guid, TaskDao> _storage = new ConcurrentDictionary<Guid, TaskDao>();

        public void Delete(Guid id)
        {
            TaskDao deleted;
            if (_storage.ContainsKey(id)) _storage.TryRemove(id, out deleted);
        }

        public List<TaskDao> GetAll()
        {
            return _storage.Values.ToList();
        }

        public TaskDao GetById(Guid id)
        {
            if (_storage.ContainsKey(id)) return _storage[id];
            return null;
        }

        public TaskDao Save(TaskDao item)
        {
            _storage[item.Id] = item;
            return item;
        }
    }
}
