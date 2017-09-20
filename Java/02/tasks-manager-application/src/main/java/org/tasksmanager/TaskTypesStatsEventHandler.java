package org.tasksmanager;

import java.util.logging.Level;
import java.util.logging.Logger;
import javax.inject.Inject;
import javax.inject.Named;
import org.commons.Services.TaskDao;
import org.commons.Services.TasksService;
import org.commons.Tasks.TaskCompleted;
import org.commons.Tasks.TaskCreated;
import org.cqrs.Bus;
import org.cqrs.MessageHandler;
import org.cqrs.Repository;
import org.tasksmanager.Repositories.TaskTypeStatDao;

@Named("taskTypesStatsEventHandler")
public class TaskTypesStatsEventHandler implements MessageHandler {

    private static final Logger logger = Logger.getLogger(TaskTypesStatsEventHandler.class.getSimpleName());
    private final TasksService _tasksService;
    private final Repository<TaskTypeStatDao, String> _repository;
    private Bus _bus;

    @Override
    public void Register(Bus bus) {
        _bus = bus;
        _bus.RegisterHandler(m -> Handle((TaskCompleted) m), TaskCompleted.class);
        _bus.RegisterHandler(m -> Handle((TaskCreated) m), TaskCreated.class);
    }

    @Inject
    public TaskTypesStatsEventHandler(TasksService tasksService, Repository<TaskTypeStatDao, String> taskTypeStatsRepository) {
        _repository = taskTypeStatsRepository;
        _tasksService = tasksService;
    }

    public void Handle(TaskCompleted message) {
        logger.log(Level.INFO, "TaskCompleted");
        TaskDao taskDao = _tasksService.GetById(message.getId());
        String code = taskDao.getTypeCode() == null ? "NONE" : taskDao.getTypeCode();

        TaskTypeStatDao currentStat = _repository.GetById(code);
        if (currentStat == null) {
            throw new RuntimeException("Missing stats for code " + code);
        }

        currentStat.setCompleted(currentStat.getCompleted() + 1);
        currentStat.setRunning(currentStat.getRunning() - 1);

        _repository.Save(currentStat);
    }

    public void Handle(TaskCreated message) {
        logger.log(Level.INFO, "TaskCreated");
        TaskDao taskDao = _tasksService.GetById(message.getId());
        String code = taskDao.getTypeCode() == null ? "NONE" : taskDao.getTypeCode();

        TaskTypeStatDao currentStat = _repository.GetById(code);
        if (currentStat == null) {
            currentStat = new TaskTypeStatDao();
            currentStat.setTypeCode(code);
        }

        currentStat.setRunning(currentStat.getRunning() + 1);

        _repository.Save(currentStat);
    }
}
