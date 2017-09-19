package org.tasks.Services;

import java.util.List;
import java.util.UUID;
import org.commons.Services.TaskDao;
import org.commons.Services.TasksService;
import org.cqrs.Repository;

public class TasksServiceImpl implements TasksService {

    private Repository<TaskDao> _repository;

    public TasksServiceImpl(Repository<TaskDao> repository) {
        _repository = repository;
    }

    public List<TaskDao> GetAll() {
        return _repository.GetAll();
    }

    public TaskDao GetById(UUID id) {
        return _repository.GetById(id);
    }
}
