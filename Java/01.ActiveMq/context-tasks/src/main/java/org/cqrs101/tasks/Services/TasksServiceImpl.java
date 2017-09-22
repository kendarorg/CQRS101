package org.cqrs101.tasks.Services;

import java.util.List;
import java.util.UUID;
import javax.inject.Named;
import org.cqrs.Repository;
import org.cqrs101.shared.Services.TaskDao;
import org.cqrs101.shared.Services.TasksService;

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
