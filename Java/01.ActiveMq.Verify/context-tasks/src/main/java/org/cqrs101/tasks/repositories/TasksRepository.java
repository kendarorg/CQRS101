package org.cqrs101.tasks.repositories;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import javax.inject.Named;
import org.cqrs101.Repository;

@Named("tasksRepository")
public class TasksRepository implements Repository<TaskDao> {

    private static final ConcurrentHashMap<UUID, TaskDao> storage = new ConcurrentHashMap<UUID, TaskDao>();

    @Override
    public void delete(UUID id) {
        if (storage.containsKey(id)) {
            storage.remove(id);
        }
    }

    @Override
    public List<TaskDao> getAll() {
        return new ArrayList<>(storage.values());
    }

    @Override
    public TaskDao getById(UUID id) {
        if (storage.containsKey(id)) {
            return storage.get(id);
        }
        return null;
    }

    @Override
    public TaskDao save(TaskDao item) {
        storage.put(item.getId(), item);
        return item;
    }
}
