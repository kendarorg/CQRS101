package org.tasks.Repositories;

import java.util.Dictionary;
import java.util.HashMap;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import org.cqrs.Repository;
import org.commons.Services.TaskDao;

public class TasksRepository implements Repository<TaskDao> {

    private static ConcurrentHashMap<UUID, TaskDao> _storage = new ConcurrentHashMap<UUID, TaskDao>();

    public void Delete(UUID id) {
        if (_storage.containsKey(id)) {
            _storage.remove(id);
        }
    }

    public List<TaskDao> GetAll() {
        return (List<TaskDao>) _storage.elements();
    }

    public TaskDao GetById(UUID id) {
        if (_storage.containsKey(id)) {
            return _storage.get(id);
        }
        return null;
    }

    public TaskDao Save(TaskDao item) {
        _storage.put(item.getId(), item);
        return item;
    }
}
