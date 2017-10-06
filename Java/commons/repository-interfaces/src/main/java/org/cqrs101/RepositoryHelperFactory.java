package org.cqrs101;

import java.util.List;
import java.util.concurrent.ConcurrentHashMap;

public class RepositoryHelperFactory {

    private final RepositoryHelper repositoryHelper;
    private final ConcurrentHashMap<Class, RepositoryHelper> repositoryHelperInstances;

    public RepositoryHelperFactory(RepositoryHelper repositoryHelper) {
        this.repositoryHelper = repositoryHelper;
        repositoryHelperInstances = new ConcurrentHashMap<>();
    }

    public RepositoryHelper getHelper(Class clazz) {
        if (!repositoryHelperInstances.containsKey(clazz)) {
            repositoryHelperInstances.putIfAbsent(clazz, repositoryHelper.create(clazz));
        }
        return repositoryHelperInstances.get(clazz);
    }
}
