package org.shared.Services;

import java.util.List;
import java.util.UUID;

public interface TasksService {

    TaskDao GetById(UUID id);
    List<TaskDao> GetAll();
}
