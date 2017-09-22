package org.cqrs101.views.Repositories;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import javax.inject.Named;
import org.cqrs.Repository;

@Named("toDoTasksRepository")
public class ToDoTasksRepository implements Repository<ToDoTaskDao> {

    private static final ConcurrentHashMap<UUID, ToDoTaskDao> _storage = new ConcurrentHashMap<UUID, ToDoTaskDao>();

    @Override
    public void Delete(UUID id) {
        if (_storage.containsKey(id)) {
            _storage.remove(id);
        }
    }

    @Override
    public List<ToDoTaskDao> GetAll() {
        return new ArrayList<>(_storage.values());
    }

    @Override
    public ToDoTaskDao GetById(UUID id) {
        if (_storage.containsKey(id)) {
            return _storage.get(id);
        }
        return null;
    }

    @Override
    public ToDoTaskDao Save(ToDoTaskDao item) {
        _storage.put(item.getId(), item);
        return item;
    }
}
