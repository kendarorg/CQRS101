package org.tasksmanager;

import java.util.UUID;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.inject.Inject;
import javax.inject.Named;
import org.commons.Tasks.*;
import org.cqrs.*;
import org.tasksmanager.Repositories.*;

@Named("toDoTasksEventHandler")
public class ToDoTasksEventHandler implements MessageHandler {

    private static final Logger logger = Logger.getLogger(ToDoTasksEventHandler.class.getSimpleName());
    private final Repository<ToDoTaskDao,UUID> _repository;
    private Bus _bus;

    @Override
    public void Register(Bus bus) {
        _bus = bus;
        _bus.RegisterHandler(m -> Handle((TaskCreated) m), TaskCreated.class);
        _bus.RegisterHandler(m -> Handle((TaskPriorityChanged) m), TaskPriorityChanged.class);
        _bus.RegisterHandler(m -> Handle((TaskTitleChanged) m), TaskTitleChanged.class);
        _bus.RegisterHandler(m -> Handle((TaskCompleted) m), TaskCompleted.class);
    }

    @Inject
    public ToDoTasksEventHandler(Repository<ToDoTaskDao,UUID> toDoTasksRepository) {
        _repository = toDoTasksRepository;
    }

    public void Handle(TaskCreated message) {
        logger.log(Level.INFO, "TaskCreated");
        ToDoTaskDao toDoTask = new ToDoTaskDao();
        toDoTask.setId(message.getId());
        toDoTask.setCreationDate(message.getCreationDate());

        _repository.Save(toDoTask);
        Handle(message.getTitleSet());
        Handle(message.getPrioritySet());
    }

    public void Handle(TaskPriorityChanged message) {
        logger.log(Level.INFO, "TaskPriorityChanged");
        ToDoTaskDao toDoTask = _repository.GetById(message.getId());
        toDoTask.setPriority(message.getNew());
        _repository.Save(toDoTask);
    }

    public void Handle(TaskTitleChanged message) {
        logger.log(Level.INFO, "TaskTitleChanged");
        ToDoTaskDao toDoTask = _repository.GetById(message.getId());
        toDoTask.setTitle(message.getNew());
        _repository.Save(toDoTask);
    }

    public void Handle(TaskCompleted message) {
        logger.log(Level.INFO, "TaskCompleted");
        _repository.Delete(message.getId());
    }
}
