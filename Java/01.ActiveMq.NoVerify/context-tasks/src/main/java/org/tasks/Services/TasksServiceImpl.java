package org.tasks.Services;

import java.util.List;
import java.util.UUID;
import javax.inject.Named;
import org.commons.Services.TaskDao;
import org.commons.Services.TasksService;
import org.cqrs.Repository;

@Named("tasksService")
public class TasksServiceImpl implements TasksService {

    private final Repository<TaskDao> _repository;

    public TasksServiceImpl(Repository<TaskDao> tasksRepository) {
        _repository = tasksRepository;
    }

    @Override
    public List<TaskDao> GetAll() {
        return _repository.GetAll();
    }

    @Override
    public TaskDao GetById(UUID id) {
        return _repository.GetById(id);
    }
}
