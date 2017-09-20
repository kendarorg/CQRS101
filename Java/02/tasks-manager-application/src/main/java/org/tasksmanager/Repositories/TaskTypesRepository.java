package org.tasksmanager.Repositories;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;
import javax.inject.Named;
import org.commons.Services.TaskTypeDao;
import org.cqrs.Repository;

@Named("taskTypesRepository")
public class TaskTypesRepository implements Repository<TaskTypeDao, String> {

    private static final ConcurrentHashMap<String, TaskTypeDao> _storage = new ConcurrentHashMap<String, TaskTypeDao>();

    @Override
    public void Delete(String id) {
        if (_storage.containsKey(id)) {
            _storage.remove(id);
        }
    }

    @Override
    public List<TaskTypeDao> GetAll() {
        return new ArrayList<>(_storage.values());
    }

    @Override
    public TaskTypeDao GetById(String id) {
        if (_storage.containsKey(id)) {
            return _storage.get(id);
        }
        return null;
    }

    @Override
    public TaskTypeDao Save(TaskTypeDao item) {
        _storage.put(item.getCode(), item);
        return item;
    }
}
