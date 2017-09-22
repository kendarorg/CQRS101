package org.cqrs101.shared.Services;

import java.util.List;
import java.util.UUID;

public interface TasksService {

    TaskServiceDao GetById(UUID id);
    List<TaskServiceDao> GetAll();
}
