package org.cqrs101.views;

import java.util.logging.Level;
import java.util.logging.Logger;
import javax.inject.Inject;
import javax.inject.Named;
import org.cqrs.Bus;
import org.cqrs.MessageHandler;
import org.cqrs101.Repository;
import org.cqrs101.shared.services.*;
import org.cqrs101.shared.tasks.*;
import org.cqrs101.views.repositories.*;

@Named("doneTasksEventHandler")
public class DoneTasksEventHandler implements MessageHandler {

    private static final Logger logger = Logger.getLogger(DoneTasksEventHandler.class.getSimpleName());
    private final TasksService tasksService;
    private final Repository<DoneTaskDao> repository;
    private Bus bus;

    @Override
    public void register(Bus bus) {
        this.bus = bus;
        this.bus.registerHandler(m -> handle((TaskCompleted) m), TaskCompleted.class);
    }

    @Inject
    public DoneTasksEventHandler(TasksService tasksService, Repository<DoneTaskDao> doneTasksRepository) {
        this.repository = doneTasksRepository;
        this.tasksService = tasksService;
    }

    public void handle(TaskCompleted message) {
        logger.log(Level.INFO, "{0}-TaskCompleted", message.getCorrelationId());
        TaskServiceDao taskDao = tasksService.getById(message.getId());
        DoneTaskDao doneTask = new DoneTaskDao();
        doneTask.setId(taskDao.getId());
        doneTask.setPriority(taskDao.getPriority());
        doneTask.setCreationDate(taskDao.getCreationDate());
        doneTask.setTitle(taskDao.getTitle());
        doneTask.setCompletionDate(taskDao.getCompletionDate());
        repository.save(doneTask);
    }
}
