package org.cqrs101.views.repositories;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import javax.inject.Named;
import org.cqrs101.Repository;

@Named("toDoTasksRepository")
public class ToDoTasksRepository implements Repository<ToDoTaskDao> {

    private static final ConcurrentHashMap<UUID, ToDoTaskDao> storage = new ConcurrentHashMap<UUID, ToDoTaskDao>();

    @Override
    public void delete(UUID id) {
        if (storage.containsKey(id)) {
            storage.remove(id);
        }
    }

    @Override
    public List<ToDoTaskDao> getAll() {
        return new ArrayList<>(storage.values());
    }

    @Override
    public ToDoTaskDao getById(UUID id) {
        if (storage.containsKey(id)) {
            return storage.get(id);
        }
        return null;
    }

    @Override
    public ToDoTaskDao save(ToDoTaskDao item) {
        storage.put(item.getId(), item);
        return item;
    }
}
