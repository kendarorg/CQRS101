package org.cqrs;

import java.util.List;

public interface Repository<T,K> {

    T GetById(K id);

    List<T> GetAll();

    T Save(T item);

    void Delete(K id);
}
