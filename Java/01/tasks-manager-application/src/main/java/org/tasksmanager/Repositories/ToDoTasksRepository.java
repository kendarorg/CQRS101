package org.tasksmanager.Repositories;

import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import org.cqrs.Repository;


    public class ToDoTasksRepository implements Repository<ToDoTaskDao>
    {
        private static ConcurrentHashMap<UUID, ToDoTaskDao> _storage = new ConcurrentHashMap<UUID, ToDoTaskDao>();

        public void Delete(UUID id)
        {
            if (_storage.containsKey(id)) _storage.remove(id);
        }

        public List<ToDoTaskDao> GetAll()
        {
            return (List<ToDoTaskDao>) _storage.elements();
        }

        public ToDoTaskDao GetById(UUID id)
        {
            if (_storage.containsKey(id)) return _storage.get(id);
            return null;
        }

        public ToDoTaskDao Save(ToDoTaskDao item)
        {
            _storage.put(item.getId(), item);
            return item;
        }
    }

