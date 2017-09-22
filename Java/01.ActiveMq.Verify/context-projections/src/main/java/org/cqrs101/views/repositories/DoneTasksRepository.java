package org.cqrs101.views.repositories;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import javax.inject.Named;
import org.cqrs101.Repository;

@Named("doneTasksRepository")
public class DoneTasksRepository implements Repository<DoneTaskDao> {

    private static final ConcurrentHashMap<UUID, DoneTaskDao> storage = new ConcurrentHashMap<UUID, DoneTaskDao>();

    @Override
    public void delete(UUID id) {
        if (storage.containsKey(id)) {
            storage.remove(id);
        }
    }

    @Override
    public List<DoneTaskDao> getAll() {
        return new ArrayList<>(storage.values());
    }

    @Override
    public DoneTaskDao getById(UUID id) {
        if (storage.containsKey(id)) {
            return storage.get(id);
        }
        return null;
    }

    @Override
    public DoneTaskDao save(DoneTaskDao item) {
        storage.put(item.getId(), item);
        return item;
    }
}
