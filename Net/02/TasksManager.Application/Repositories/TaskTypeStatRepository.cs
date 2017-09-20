using Commons.Services;
using Cqrs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace TasksManager.Repositories
{
    public class TaskTypeStatRepository : IRepository<TaskTypeStatDao, String>
    {
        private static ConcurrentDictionary<String, TaskTypeStatDao> _storage = new ConcurrentDictionary<String, TaskTypeStatDao>();

        public void Delete(String id)
        {
            TaskTypeStatDao deleted;
            if (_storage.ContainsKey(id)) _storage.TryRemove(id, out deleted);
        }

        public List<TaskTypeStatDao> GetAll()
        {
            return _storage.Values.ToList();
        }

        public TaskTypeStatDao GetById(String id)
        {
            if (_storage.ContainsKey(id)) return _storage[id];
            return null;
        }

        public TaskTypeStatDao Save(TaskTypeStatDao item)
        {
            _storage[item.TypeCode] = item;
            return item;
        }
    }
}
