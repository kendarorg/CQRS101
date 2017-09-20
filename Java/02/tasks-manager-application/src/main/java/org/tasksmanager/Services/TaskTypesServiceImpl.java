package org.tasksmanager.Services;

import javax.inject.Inject;
import javax.inject.Named;
import org.commons.Services.TaskTypeDao;
import org.commons.Services.TaskTypesService;
import org.cqrs.Repository;

@Named("taskTypesService")
public class TaskTypesServiceImpl implements TaskTypesService {

    private Repository<TaskTypeDao, String> _taskTypesRepository;

    @Inject
    public TaskTypesServiceImpl(Repository<TaskTypeDao, String> taskTypesRepository) {
        _taskTypesRepository = taskTypesRepository;
    }

    @Override
    public TaskTypeDao GetById(String code) {
        return _taskTypesRepository.GetById(code);
    }

}
