package org.cqrs101;

import java.lang.reflect.ParameterizedType;
import java.util.List;
import java.util.UUID;
import java.util.stream.Collectors;

@SuppressWarnings("unchecked")
public abstract class BaseRepository<T> implements Repository<T> {
    protected final RepositoryHelper helper;

    protected BaseRepository(RepositoryHelper helper) {
        Class<T> clazz = (Class<T>) ((ParameterizedType) getClass()
                .getGenericSuperclass()).getActualTypeArguments()[0];
        this.helper = helper.create(clazz);
    }

    @Override
    public void delete(UUID id) {
        helper.delete(id);
    }

    @Override
    public List<T> getAll() {
        return helper.getAll()
                .stream()
                .map(c -> (T) c)
                .collect(Collectors.toList());
    }

    @Override
    public T getById(UUID id) {
        return (T)helper.getById(id);}

//    @Override
//    public T save(T item) {
//        return (T) helper.save(item,
//                (obj, id) -> ((T) obj).setId(id),
//                (obj) -> ((T) obj).getId());
//    }
}
