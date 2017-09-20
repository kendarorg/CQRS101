package org.tasksmanager;

import java.util.logging.Level;
import java.util.logging.Logger;
import javax.inject.Named;
import org.commons.Services.TaskDao;
import org.commons.Services.TasksService;
import org.commons.Tasks.TaskCompleted;
import org.cqrs.Bus;
import org.cqrs.MessageHandler;
import org.cqrs.Repository;
import org.tasksmanager.Repositories.DoneTaskDao;

@Named("doneTasksEventHandler")
public class DoneTasksEventHandler implements MessageHandler {

    private static final Logger logger = Logger.getLogger(DoneTasksEventHandler.class.getSimpleName());
    private final TasksService _tasksService;
    private final Repository<DoneTaskDao> _repository;
    private Bus _bus;

    @Override
    public void Register(Bus bus) {
        _bus = bus;
        _bus.RegisterHandler(m -> Handle((TaskCompleted) m), TaskCompleted.class);
    }

    public DoneTasksEventHandler(TasksService tasksService, Repository<DoneTaskDao> doneTasksRepository) {
        _repository = doneTasksRepository;
        _tasksService = tasksService;
    }

    public void Handle(TaskCompleted message) {
        logger.log(Level.INFO, "TaskCompleted");
        TaskDao taskDao = _tasksService.GetById(message.getId());
        DoneTaskDao doneTask = new DoneTaskDao();
        doneTask.setId(taskDao.getId());
        doneTask.setPriority(taskDao.getPriority());
        doneTask.setCreationDate(taskDao.getCreationDate());
        doneTask.setTitle(taskDao.getTitle());
        doneTask.setCompletionDate(taskDao.getCompletionDate());
        _repository.Save(doneTask);
    }
}
