package org.cqrs101.tasks;

import org.cqrs101.tasks.commands.ChangeTaskPriority;
import org.cqrs101.tasks.commands.ChangeTaskDescription;
import org.cqrs101.tasks.commands.CompleteTask;
import org.cqrs101.tasks.commands.CreateTask;
import org.cqrs101.tasks.commands.ChangeTaskTitle;
import org.cqrs101.tasks.commands.VerifyTaskTitle;
import org.cqrs101.tasks.commands.AddTaskHours;
import org.cqrs101.shared.tasks.TaskCompleted;
import org.cqrs101.shared.tasks.TaskTitleChanged;
import org.cqrs101.shared.tasks.TaskDescriptionChanged;
import org.cqrs101.shared.tasks.TaskHoursAdded;
import org.cqrs101.shared.tasks.TaskPriorityChanged;
import org.cqrs101.shared.tasks.TaskCreated;
import org.cqrs101.shared.tasks.TaskTitleVerified;
import org.cqrs101.tasks.repositories.TaskDao;
import java.util.Date;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.inject.Named;
import org.cqrs.*;
import org.cqrs101.Repository;

@Named("tasksCommandHandler")
public class TasksCommandHandler implements MessageHandler {

    private static final Logger logger = Logger.getLogger(TasksCommandHandler.class.getSimpleName());
    private final Repository<TaskDao> repository;
    private Bus bus;

    @Override
    public void register(Bus bus) {
        this.bus = bus;
        this.bus.registerHandler(c -> handle((CreateTask) c), CreateTask.class);
        this.bus.registerHandler(c -> handle((CompleteTask) c), CompleteTask.class);

        this.bus.registerHandler(c -> handle((ChangeTaskTitle) c), ChangeTaskTitle.class);
        this.bus.registerHandler(c -> handle((ChangeTaskDescription) c), ChangeTaskDescription.class);
        this.bus.registerHandler(c -> handle((AddTaskHours) c), AddTaskHours.class);
        this.bus.registerHandler(c -> handle((ChangeTaskPriority) c), ChangeTaskPriority.class);

        this.bus.registerHandler(c -> handle((TaskTitleVerified) c), TaskTitleVerified.class);
    }

    public TasksCommandHandler(Repository<TaskDao> tasksRepository) {
        this.repository = tasksRepository;
    }

    public void handle(CreateTask command) {
        logger.log(Level.INFO, "{0}-CreateTask", command.getCorrelationId());
        Date now = new Date();
        TaskDao taskDao = new TaskDao();
        taskDao.setId(command.getId());
        taskDao.setDescription(command.getDescription());
        taskDao.setPriority(command.getPriority());
        taskDao.setTitle(command.getTitle());
        taskDao.setCompleted(false);
        taskDao.setCreationDate(now);

        repository.save(taskDao);
        VerifyTaskTitle message = new VerifyTaskTitle();
        message.setId(command.getId());
        message.setTitle(command.getTitle());
        message.setCorrelationId(command.getCorrelationId());
        bus.send(message);
    }

    public void handle(ChangeTaskPriority command) {
        logger.log(Level.INFO, "{0}-ChangeTaskPriority", command.getCorrelationId());
        TaskDao taskDao = repository.getById(command.getId());
        TaskPriorityChanged message = new TaskPriorityChanged(command.getId(), command.getPriority());
        message.setOld(taskDao.getPriority());

        taskDao.setPriority(command.getPriority());
        repository.save(taskDao);
        bus.send(message);
    }

    public void handle(AddTaskHours command) {
        logger.log(Level.INFO, "{0}-AddTaskHours", command.getCorrelationId());
        TaskDao taskDao = repository.getById(command.getId());
        TaskHoursAdded message = new TaskHoursAdded(command.getId(), command.getTaskHours());
        message.setOld(taskDao.getHours());
        taskDao.setHours(taskDao.getHours() + command.getTaskHours());
        repository.save(taskDao);
        message.setCorrelationId(command.getCorrelationId());
        bus.send(message);
    }

    public void handle(ChangeTaskDescription command) {
        logger.log(Level.INFO, "{0}-ChangeTaskDescription", command.getCorrelationId());
        TaskDao taskDao = repository.getById(command.getId());
        TaskDescriptionChanged message = new TaskDescriptionChanged(command.getId(), command.getDescription());
        message.setOld(taskDao.getDescription());

        taskDao.setDescription(command.getDescription());
        repository.save(taskDao);
        message.setCorrelationId(command.getCorrelationId());
        bus.send(message);
    }

    public void handle(ChangeTaskTitle command) {
        logger.log(Level.INFO, "{0}-ChangeTaskTitle", command.getCorrelationId());
        TaskDao taskDao = repository.getById(command.getId());
        TaskTitleChanged message = new TaskTitleChanged(command.getId(), command.getTitle());
        message.setOld(taskDao.getTitle());

        taskDao.setTitle(command.getTitle());
        repository.save(taskDao);
        message.setCorrelationId(command.getCorrelationId());
        bus.send(message);
    }

    public void handle(CompleteTask command) {
        logger.log(Level.INFO, "{0}-CompleteTask", command.getCorrelationId());
        Date now = new Date();
        TaskDao taskDao = repository.getById(command.getId());
        taskDao.setCompleted(true);
        taskDao.setCompletionDate(now);

        TaskCompleted message = new TaskCompleted(taskDao.getId(), now);
        message.setCorrelationId(command.getCorrelationId());
        bus.send(message);
    }

    public void handle(TaskTitleVerified command) {
        logger.log(Level.INFO, "{0}-TaskTitleVerified", command.getCorrelationId());
        TaskDao taskDao = repository.getById(command.getId());
        taskDao.setInitialized(true);
        repository.save(taskDao);

        TaskCreated message = new TaskCreated();
        message.setId(taskDao.getId());
        message.setCreationDate(taskDao.getCreationDate());
        message.setTitleSet(new TaskTitleChanged(taskDao.getId(), taskDao.getTitle()));
        message.setDescriptionSet(new TaskDescriptionChanged(taskDao.getId(), taskDao.getDescription()));
        message.setPrioritySet(new TaskPriorityChanged(taskDao.getId(), taskDao.getPriority()));

        message.setCorrelationId(command.getCorrelationId());
        bus.send(message);
    }
}
