package org.cqrs101.shared.services;

import java.util.List;
import java.util.UUID;

public interface TasksService {

    TaskServiceDao getById(UUID id);
    List<TaskServiceDao> getAll();
}
