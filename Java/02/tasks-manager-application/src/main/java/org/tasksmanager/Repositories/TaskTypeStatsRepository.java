package org.tasksmanager.Repositories;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;
import javax.inject.Named;
import org.cqrs.Repository;

@Named("taskTypeStatsRepository")
public class TaskTypeStatsRepository implements Repository<TaskTypeStatDao, String> {

    private static final ConcurrentHashMap<String, TaskTypeStatDao> _storage = new ConcurrentHashMap<String, TaskTypeStatDao>();

    @Override
    public void Delete(String id) {
        if (_storage.containsKey(id)) {
            _storage.remove(id);
        }
    }

    @Override
    public List<TaskTypeStatDao> GetAll() {
        return new ArrayList<>(_storage.values());
    }

    @Override
    public TaskTypeStatDao GetById(String id) {
        if (_storage.containsKey(id)) {
            return _storage.get(id);
        }
        return null;
    }

    @Override
    public TaskTypeStatDao Save(TaskTypeStatDao item) {
        _storage.put(item.getTypeCode(), item);
        return item;
    }
}
