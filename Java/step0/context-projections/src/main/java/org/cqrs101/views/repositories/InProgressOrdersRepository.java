package org.cqrs101.views.repositories;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import javax.inject.Named;
import org.cqrs101.Repository;

@Named("inProgressOrdersRepository")
public class InProgressOrdersRepository implements Repository<InProgressOrder> {

    private static final ConcurrentHashMap<UUID, InProgressOrder> storage = new ConcurrentHashMap<UUID, InProgressOrder>();

    @Override
    public void delete(UUID id) {
        if (storage.containsKey(id)) {
            storage.remove(id);
        }
    }

    @Override
    public List<InProgressOrder> getAll() {
        return new ArrayList<>(storage.values());
    }

    @Override
    public InProgressOrder getById(UUID id) {
        if (storage.containsKey(id)) {
            return storage.get(id);
        }
        return null;
    }

    @Override
    public InProgressOrder save(InProgressOrder item) {
        storage.put(item.getId(), item);
        return item;
    }
}
