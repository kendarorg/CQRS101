using Commons.Services;
using Cqrs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace TasksManager.Repositories
{
    public class TaskTypesRepository : IRepository<TaskTypeDao, String>
    {
        private static ConcurrentDictionary<String, TaskTypeDao> _storage = new ConcurrentDictionary<String, TaskTypeDao>();

        public void Delete(String id)
        {
            TaskTypeDao deleted;
            if (_storage.ContainsKey(id)) _storage.TryRemove(id, out deleted);
        }

        public List<TaskTypeDao> GetAll()
        {
            return _storage.Values.ToList();
        }

        public TaskTypeDao GetById(String id)
        {
            if (_storage.ContainsKey(id)) return _storage[id];
            return null;
        }

        public TaskTypeDao Save(TaskTypeDao item)
        {
            _storage[item.Code] = item;
            return item;
        }
    }
}
