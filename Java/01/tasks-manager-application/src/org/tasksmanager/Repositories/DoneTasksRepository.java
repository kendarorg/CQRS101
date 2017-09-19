package org.tasksmanager.Repositories;

import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import org.cqrs.Repository;


    public class DoneTasksRepository implements Repository<DoneTaskDao>
    {
        private static ConcurrentHashMap<UUID, DoneTaskDao> _storage = new ConcurrentHashMap<UUID, DoneTaskDao>();

        public void Delete(UUID id)
        {
            if (_storage.containsKey(id)) _storage.remove(id);
        }

        public List<DoneTaskDao> GetAll()
        {
            return (List<DoneTaskDao>) _storage.elements();
        }

        public DoneTaskDao GetById(UUID id)
        {
            if (_storage.containsKey(id)) return _storage.get(id);
            return null;
        }

        public DoneTaskDao Save(DoneTaskDao item)
        {
            _storage.put(item.getId(), item);
            return item;
        }
    }

