package org.cqrs101.views.Repositories;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import javax.inject.Named;
import org.cqrs101.Repository;

@Named("doneTasksRepository")
public class DoneTasksRepository implements Repository<DoneTaskDao> {

    private static final ConcurrentHashMap<UUID, DoneTaskDao> _storage = new ConcurrentHashMap<UUID, DoneTaskDao>();

    @Override
    public void Delete(UUID id) {
        if (_storage.containsKey(id)) {
            _storage.remove(id);
        }
    }

    @Override
    public List<DoneTaskDao> GetAll() {
        return new ArrayList<>(_storage.values());
    }

    @Override
    public DoneTaskDao GetById(UUID id) {
        if (_storage.containsKey(id)) {
            return _storage.get(id);
        }
        return null;
    }

    @Override
    public DoneTaskDao Save(DoneTaskDao item) {
        _storage.put(item.getId(), item);
        return item;
    }
}
