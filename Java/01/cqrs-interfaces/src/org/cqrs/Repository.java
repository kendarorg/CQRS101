package org.cqrs;

import java.util.List;
import java.util.UUID;

    public interface Repository<T>
    {
        T GetById(UUID id);
        List<T> GetAll();
        T Save(T item);
        void Delete(UUID id);
    }

