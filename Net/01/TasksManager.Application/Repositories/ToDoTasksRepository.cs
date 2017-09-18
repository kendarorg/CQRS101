using Cqrs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManager.Repositories
{
    public class ToDoTasksRepository : IRepository<ToDoTaskDao>
    {
        private static ConcurrentDictionary<Guid, ToDoTaskDao> _storage = new ConcurrentDictionary<Guid, ToDoTaskDao>();

        public void Delete(Guid id)
        {
            ToDoTaskDao deleted;
            if (_storage.ContainsKey(id)) _storage.TryRemove(id, out deleted);
        }

        public List<ToDoTaskDao> GetAll()
        {
            return _storage.Values.ToList();
        }

        public ToDoTaskDao GetById(Guid id)
        {
            if (_storage.ContainsKey(id)) return _storage[id];
            return null;
        }

        public ToDoTaskDao Save(ToDoTaskDao item)
        {
            _storage[item.Id] = item;
            return item;
        }
    }
}
