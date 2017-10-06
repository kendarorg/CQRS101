package org.cqrs101;

import java.util.List;
import java.util.UUID;
import java.util.function.BiConsumer;
import java.util.function.Function;

public interface RepositoryHelper {

    RepositoryHelper create(Class clazz);

    String getName();

    Object getById(UUID id);

    List<Object> getAll();

    public Object save(Object item, BiConsumer<Object, UUID> idSetter, Function<Object, UUID> idGetter);

    void delete(UUID id);
}
