package org.cqrs101.views.repositories;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import javax.inject.Named;
import org.cqrs101.Repository;

@Named("inProgressOrdersRepository")
public class CompletedOrdersRepository implements Repository<CompletedOrder> {

    private static final ConcurrentHashMap<UUID, CompletedOrder> storage = new ConcurrentHashMap<UUID, CompletedOrder>();

    @Override
    public void delete(UUID id) {
        if (storage.containsKey(id)) {
            storage.remove(id);
        }
    }

    @Override
    public List<CompletedOrder> getAll() {
        return new ArrayList<>(storage.values());
    }

    @Override
    public CompletedOrder getById(UUID id) {
        if (storage.containsKey(id)) {
            return storage.get(id);
        }
        return null;
    }

    @Override
    public CompletedOrder save(CompletedOrder item) {
        storage.put(item.getId(), item);
        return item;
    }
}
