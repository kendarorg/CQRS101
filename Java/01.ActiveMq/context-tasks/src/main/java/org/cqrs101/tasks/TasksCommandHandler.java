package org.cqrs101.tasks;

import org.cqrs101.tasks.Commands.ChangeTaskPriority;
import org.cqrs101.tasks.Commands.ChangeTaskDescription;
import org.cqrs101.tasks.Commands.CompleteTask;
import org.cqrs101.tasks.Commands.CreateTask;
import org.cqrs101.tasks.Commands.ChangeTaskTitle;
import org.cqrs101.tasks.Commands.VerifyTaskTitle;
import org.cqrs101.tasks.Commands.ChangeTaskDueDate;
import org.cqrs101.shared.Tasks.TaskCompleted;
import org.cqrs101.shared.Tasks.TaskTitleChanged;
import org.cqrs101.shared.Tasks.TaskDescriptionChanged;
import org.cqrs101.shared.Tasks.TaskDueDateChanged;
import org.cqrs101.shared.Tasks.TaskPriorityChanged;
import org.cqrs101.shared.Tasks.TaskCreated;
import org.cqrs101.shared.Tasks.TaskTitleVerified;
import org.cqrs101.tasks.Repositories.TaskDao;
import java.util.Date;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.inject.Named;
import org.cqrs.*;
import org.cqrs101.Repository;

@Named("tasksCommandHandler")
public class TasksCommandHandler implements MessageHandler {

    private static final Logger logger = Logger.getLogger(TasksCommandHandler.class.getSimpleName());
    private final Repository<TaskDao> _repository;
    private Bus _bus;

    @Override
    public void Register(Bus bus) {
        _bus = bus;
        _bus.RegisterHandler(c -> Handle((CreateTask) c), CreateTask.class);
        _bus.RegisterHandler(c -> Handle((CompleteTask) c), CompleteTask.class);

        _bus.RegisterHandler(c -> Handle((ChangeTaskTitle) c), ChangeTaskTitle.class);
        _bus.RegisterHandler(c -> Handle((ChangeTaskDescription) c), ChangeTaskDescription.class);
        _bus.RegisterHandler(c -> Handle((ChangeTaskDueDate) c), ChangeTaskDueDate.class);
        _bus.RegisterHandler(c -> Handle((ChangeTaskPriority) c), ChangeTaskPriority.class);
        
        _bus.RegisterHandler(c -> Handle((TaskTitleVerified) c), TaskTitleVerified.class);
    }

    public TasksCommandHandler(Repository<TaskDao> tasksRepository) {
        _repository = tasksRepository;
    }

    public void Handle(CreateTask command) {
        logger.log(Level.INFO, "CreateTask");
        Date now = new Date();
        TaskDao taskDao = new TaskDao();
        taskDao.setId(command.getId());
        taskDao.setDescription(command.getDescription());
        taskDao.setDueDate(command.getDueDate());
        taskDao.setPriority(command.getPriority());
        taskDao.setTitle(command.getTitle());
        taskDao.setCompleted(false);
        taskDao.setCreationDate(now);

        _repository.Save(taskDao);
        VerifyTaskTitle message = new VerifyTaskTitle();
        message.setId(command.getId());
        message.setTitle(command.getTitle());
        _bus.Send(message);
    }

    public void Handle(ChangeTaskPriority command) {
        logger.log(Level.INFO, "ChangeTaskPriority");
        TaskDao taskDao = _repository.GetById(command.getId());
        TaskPriorityChanged message = new TaskPriorityChanged(command.getId(), command.getPriority());
        message.setOld(taskDao.getPriority());

        taskDao.setPriority(command.getPriority());
        _repository.Save(taskDao);
        _bus.Send(message);
    }

    public void Handle(ChangeTaskDueDate command) {
        logger.log(Level.INFO, "ChangeTaskDueDate");
        TaskDao taskDao = _repository.GetById(command.getId());
        TaskDueDateChanged message = new TaskDueDateChanged(command.getId(), command.getDueDate());
        message.setOld(taskDao.getDueDate());
        taskDao.setDueDate(command.getDueDate());
        _repository.Save(taskDao);
        _bus.Send(message);
    }

    public void Handle(ChangeTaskDescription command) {
        logger.log(Level.INFO, "ChangeTaskDescription");
        TaskDao taskDao = _repository.GetById(command.getId());
        TaskDescriptionChanged message = new TaskDescriptionChanged(command.getId(), command.getDescription());
        message.setOld(taskDao.getDescription());

        taskDao.setDescription(command.getDescription());
        _repository.Save(taskDao);
        _bus.Send(message);
    }

    public void Handle(ChangeTaskTitle command) {
        logger.log(Level.INFO, "ChangeTaskTitle");
        TaskDao taskDao = _repository.GetById(command.getId());
        TaskTitleChanged message = new TaskTitleChanged(command.getId(), command.getTitle());
        message.setOld(taskDao.getTitle());

        taskDao.setTitle(command.getTitle());
        _repository.Save(taskDao);
        _bus.Send(message);
    }

    public void Handle(CompleteTask command) {
        logger.log(Level.INFO, "CompleteTask");
        Date now = new Date();
        TaskDao taskDao = _repository.GetById(command.getId());
        taskDao.setCompleted(true);
        taskDao.setCompletionDate(now);

        _bus.Send(new TaskCompleted(taskDao.getId(), now));
    }

    private void Handle(TaskTitleVerified command) {
        logger.log(Level.INFO, "TaskTitleVerified");
        TaskDao taskDao = _repository.GetById(command.getId());
        taskDao.setInitialized(true);
        _repository.Save(taskDao);
        
        TaskCreated message = new TaskCreated();
        message.setId(taskDao.getId());
        message.setCreationDate(taskDao.getCreationDate());
        message.setTitleSet(new TaskTitleChanged(taskDao.getId(), taskDao.getTitle()));
        message.setDescriptionSet(new TaskDescriptionChanged(taskDao.getId(), taskDao.getDescription()));
        message.setPrioritySet(new TaskPriorityChanged(taskDao.getId(), taskDao.getPriority()));
        message.setDueDateSet(new TaskDueDateChanged(taskDao.getId(), taskDao.getDueDate()));

        _bus.Send(message);
    }
}
