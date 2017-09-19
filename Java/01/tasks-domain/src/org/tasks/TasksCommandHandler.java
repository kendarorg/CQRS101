package org.tasks;

import java.util.Date;
import org.commons.Services.*;
import org.commons.Tasks.*;
import org.cqrs.*;
import org.tasks.Commands.*;

public class TasksCommandHandler implements MessageHandler {

    private Repository<TaskDao> _repository;
    private Bus _bus;

    public void Register(Bus bus) {
        _bus = bus;
        _bus.RegisterHandler(c -> Handle((CreateTask) c), CreateTask.class);
        _bus.RegisterHandler(c -> Handle((CompleteTask) c), CompleteTask.class);

        _bus.RegisterHandler(c -> Handle((ChangeTaskTitle) c), ChangeTaskTitle.class);
        _bus.RegisterHandler(c -> Handle((ChangeTaskDescription) c), ChangeTaskDescription.class);
        _bus.RegisterHandler(c -> Handle((ChangeTaskDueDate) c), ChangeTaskDueDate.class);
        _bus.RegisterHandler(c -> Handle((ChangeTaskPriority) c), ChangeTaskPriority.class);
    }

    public TasksCommandHandler(Repository<TaskDao> repository) {
        _repository = repository;
    }

    public void Handle(CreateTask command) {
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

        TaskCreated message = new TaskCreated();
        message.setId(taskDao.getId());
        message.setCreationDate(taskDao.getCreationDate());
        message.setTitleSet(new TaskTitleChanged(taskDao.getId(), taskDao.getTitle()));
        message.setDescriptionSet(new TaskDescriptionChanged(taskDao.getId(), taskDao.getDescription()));
        message.setPrioritySet(new TaskPriorityChanged(taskDao.getId(), taskDao.getPriority()));
        message.setDueDateSet(new TaskDueDateChanged(taskDao.getId(), taskDao.getDueDate()));

        _bus.Send(message);
    }

    public void Handle(ChangeTaskPriority command) {
        TaskDao taskDao = _repository.GetById(command.getId());
        TaskPriorityChanged message = new TaskPriorityChanged(command.getId(), command.getPriority());
        message.setOld(taskDao.getPriority());

        taskDao.setPriority(command.getPriority());
        _repository.Save(taskDao);
        _bus.Send(message);
    }

    public void Handle(ChangeTaskDueDate command) {
        TaskDao taskDao = _repository.GetById(command.getId());
        TaskDueDateChanged message = new TaskDueDateChanged(command.getId(), command.getDueDate());
        message.setOld(taskDao.getDueDate());
        taskDao.setDueDate(command.getDueDate());
        _repository.Save(taskDao);
        _bus.Send(message);
    }

    public void Handle(ChangeTaskDescription command) {
        TaskDao taskDao = _repository.GetById(command.getId());
        TaskDescriptionChanged message = new TaskDescriptionChanged(command.getId(), command.getDescription());
        message.setOld(taskDao.getDescription());

        taskDao.setDescription(command.getDescription());
        _repository.Save(taskDao);
        _bus.Send(message);
    }

    public void Handle(ChangeTaskTitle command) {
        TaskDao taskDao = _repository.GetById(command.getId());
        TaskTitleChanged message = new TaskTitleChanged(command.getId(), command.getTitle());
        message.setOld(taskDao.getTitle());

        taskDao.setTitle(command.getTitle());
        _repository.Save(taskDao);
        _bus.Send(message);
    }

    public void Handle(CompleteTask command) {
        Date now = new Date();
        TaskDao taskDao = _repository.GetById(command.getId());
        taskDao.setCompleted(true);
        taskDao.setCompletionDate(now);

        _bus.Send(new TaskCompleted(taskDao.getId(), now));
    }
}
