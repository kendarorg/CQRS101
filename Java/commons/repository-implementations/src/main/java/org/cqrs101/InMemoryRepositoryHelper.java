package org.cqrs101;


import org.springframework.core.env.Environment;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;
import java.util.Locale;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import java.util.function.BiConsumer;
import java.util.function.Function;
import java.util.stream.Collectors;

public class InMemoryRepositoryHelper implements RepositoryHelper {

    private static final ConcurrentHashMap<Class, ConcurrentHashMap<UUID, Object>> storage = new ConcurrentHashMap<>();
    private final Environment environment;
    private Class clazz;
    private String name;
    
    public InMemoryRepositoryHelper(Environment environment){
        this.environment = environment;
    }

    @Override
    public RepositoryHelper create(Class clazz) {
        storage.putIfAbsent(clazz, new ConcurrentHashMap<>());
        InMemoryRepositoryHelper helper = new InMemoryRepositoryHelper(environment);
        helper.clazz = clazz;
        helper.name = clazz.getSimpleName().toUpperCase(Locale.ROOT).toUpperCase(Locale.ROOT);
        return helper;
    }

    @Override
    public String getName() {
        return name;
    }

    @Override
    public Object getById(UUID id) {
        ConcurrentHashMap<UUID, Object> data = storage.get(clazz);
        return data.getOrDefault(id, null);
    }

    @Override
    public List<Object> getAll() {
        ConcurrentHashMap<UUID, Object> data = storage.get(clazz);
        return new ArrayList<Object>(data.values());
    }

    @Override
    public Object save(Object item, BiConsumer<Object, UUID> idSetter, Function<Object, UUID> idGetter) {
        UUID id =idGetter.apply(item);
        if(id == null){
            id = UUID.randomUUID();
            idSetter.accept(item, id);
        }
        ConcurrentHashMap<UUID, Object> data = storage.get(clazz);
        data.put(id, item);
        return item;
    }

    @Override
    public void delete(UUID id) {
        ConcurrentHashMap<UUID, Object> data = storage.get(clazz);
        data.remove(id);
    }
}
