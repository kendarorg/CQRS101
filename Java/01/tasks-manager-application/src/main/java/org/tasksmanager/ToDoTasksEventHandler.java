package org.tasksmanager;

import org.commons.Services.*;
import org.commons.Tasks.*;
import org.cqrs.*;
import org.tasksmanager.Repositories.*;

public class ToDoTasksEventHandler implements MessageHandler {

    private Repository<ToDoTaskDao> _repository;
    private Bus _bus;

    public void Register(Bus bus) {
        _bus = bus;
        _bus.RegisterHandler(m -> Handle((TaskCreated) m), TaskCreated.class);
        _bus.RegisterHandler(m -> Handle((TaskPriorityChanged) m), TaskPriorityChanged.class);
        _bus.RegisterHandler(m -> Handle((TaskTitleChanged) m), TaskTitleChanged.class);
        _bus.RegisterHandler(m -> Handle((TaskCompleted) m), TaskCompleted.class);
    }

    public ToDoTasksEventHandler(Repository<ToDoTaskDao> repository) {
        _repository = repository;
    }

    public void Handle(TaskCreated message) {
        ToDoTaskDao toDoTask = new ToDoTaskDao();
        toDoTask.setId(message.getId());
        toDoTask.setCreationDate(message.getCreationDate());

        _repository.Save(toDoTask);
        Handle(message.getTitleSet());
        Handle(message.getPrioritySet());
    }

    public void Handle(TaskPriorityChanged message) {
        ToDoTaskDao toDoTask = _repository.GetById(message.getId());
        toDoTask.setPriority(message.getNew());
        _repository.Save(toDoTask);
    }

    public void Handle(TaskTitleChanged message) {
        ToDoTaskDao toDoTask = _repository.GetById(message.getId());
        toDoTask.setTitle(message.getNew());
        _repository.Save(toDoTask);
    }

    public void Handle(TaskCompleted message) {
        _repository.Delete(message.getId());
    }
}
