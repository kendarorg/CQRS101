package org.cqrs101.views;

import org.cqrs101.views.repositories.ToDoTaskDao;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.inject.Inject;
import javax.inject.Named;
import org.cqrs.*;
import org.cqrs101.Repository;
import org.cqrs101.shared.tasks.*;

@Named("toDoTasksEventHandler")
public class ToDoTasksEventHandler implements MessageHandler {

    private static final Logger logger = Logger.getLogger(ToDoTasksEventHandler.class.getSimpleName());
    private final Repository<ToDoTaskDao> repository;
    private Bus bus;

    @Override
    public void register(Bus bus) {
        this.bus = bus;
        this.bus.registerHandler(m -> handle((TaskCreated) m), TaskCreated.class);
        this.bus.registerHandler(m -> handle((TaskPriorityChanged) m), TaskPriorityChanged.class);
        this.bus.registerHandler(m -> handle((TaskTitleChanged) m), TaskTitleChanged.class);
        this.bus.registerHandler(m -> handle((TaskCompleted) m), TaskCompleted.class);
    }

    @Inject
    public ToDoTasksEventHandler(Repository<ToDoTaskDao> toDoTasksRepository) {
        this.repository = toDoTasksRepository;
    }

    public void handle(TaskCreated message) {
        logger.log(Level.INFO, "{0}-TaskCreated", message.getCorrelationId());
        ToDoTaskDao toDoTask = new ToDoTaskDao();
        toDoTask.setId(message.getId());
        toDoTask.setCreationDate(message.getCreationDate());

        repository.save(toDoTask);
        handle(message.getTitleSet());
        handle(message.getPrioritySet());
    }

    public void handle(TaskPriorityChanged message) {
        logger.log(Level.INFO, "{0}-TaskPriorityChanged", message.getCorrelationId());
        ToDoTaskDao toDoTask = repository.getById(message.getId());
        toDoTask.setPriority(message.getNew());
        repository.save(toDoTask);
    }

    public void handle(TaskTitleChanged message) {
        logger.log(Level.INFO, "{0}-TaskTitleChanged", message.getCorrelationId());
        ToDoTaskDao toDoTask = repository.getById(message.getId());
        toDoTask.setTitle(message.getNew());
        repository.save(toDoTask);
    }

    public void handle(TaskCompleted message) {
        logger.log(Level.INFO, "{0}-TaskCompleted", message.getCorrelationId());
        repository.delete(message.getId());
    }
}
