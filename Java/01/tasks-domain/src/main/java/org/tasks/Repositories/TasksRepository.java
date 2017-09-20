package org.tasks.Repositories;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import javax.inject.Named;
import org.cqrs.Repository;
import org.commons.Services.TaskDao;

@Named("tasksRepository")
public class TasksRepository implements Repository<TaskDao> {

    private static final ConcurrentHashMap<UUID, TaskDao> _storage = new ConcurrentHashMap<UUID, TaskDao>();

    @Override
    public void Delete(UUID id) {
        if (_storage.containsKey(id)) {
            _storage.remove(id);
        }
    }

    @Override
    public List<TaskDao> GetAll() {
        return new ArrayList<>(_storage.values());
    }

    @Override
    public TaskDao GetById(UUID id) {
        if (_storage.containsKey(id)) {
            return _storage.get(id);
        }
        return null;
    }

    @Override
    public TaskDao Save(TaskDao item) {
        _storage.put(item.getId(), item);
        return item;
    }
}
