package org.cqrs101;

import java.util.List;
import java.util.UUID;

public interface Repository<T> {

    T getById(UUID id);

    List<T> getAll();

    T save(T item);

    void delete(UUID id);
}
