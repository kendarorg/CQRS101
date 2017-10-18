package org.cqrs101.views.repositories;

import org.cqrs101.Repository;

import javax.inject.Named;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;

@Named("canceledOrdersRepository")
public class CanceledOrdersRepository implements Repository<CanceledOrder> {

    private static final ConcurrentHashMap<UUID, CanceledOrder> storage = new ConcurrentHashMap<UUID, CanceledOrder>();

    @Override
    public void delete(UUID id) {
        if (storage.containsKey(id)) {
            storage.remove(id);
        }
    }

    @Override
    public List<CanceledOrder> getAll() {
        return new ArrayList<>(storage.values());
    }

    @Override
    public CanceledOrder getById(UUID id) {
        if (storage.containsKey(id)) {
            return storage.get(id);
        }
        return null;
    }

    @Override
    public CanceledOrder save(CanceledOrder item) {
        storage.put(item.getId(), item);
        return item;
    }
}
