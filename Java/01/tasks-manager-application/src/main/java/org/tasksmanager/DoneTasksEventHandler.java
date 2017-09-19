package org.tasksmanager;

import org.commons.Services.TaskDao;
import org.commons.Services.TasksService;
import org.commons.Tasks.TaskCompleted;
import org.cqrs.Bus;
import org.cqrs.MessageHandler;
import org.cqrs.Repository;
import org.tasksmanager.Repositories.DoneTaskDao;

public class DoneTasksEventHandler implements MessageHandler {

    private TasksService _tasksService;
    private Repository<DoneTaskDao> _repository;
    private Bus _bus;

    public void Register(Bus bus) {
        _bus = bus;
        _bus.RegisterHandler(m -> Handle((TaskCompleted) m), TaskCompleted.class);
    }

    public DoneTasksEventHandler(TasksService tasksService, Repository<DoneTaskDao> repository) {
        _repository = repository;
        _tasksService = tasksService;
    }

    public void Handle(TaskCompleted message) {
        TaskDao taskDao = _tasksService.GetById(message.getId());
        DoneTaskDao doneTask = new DoneTaskDao();
        doneTask.setId(taskDao.getId());
        doneTask.setPriority(taskDao.getPriority());
        doneTask.setCreationDate(taskDao.getCreationDate());
        doneTask.setTitle(taskDao.getTitle());
        doneTask.setCompletionDate(taskDao.getCompletionDate());
    }
}
