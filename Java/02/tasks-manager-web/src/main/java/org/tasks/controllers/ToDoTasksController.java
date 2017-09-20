package org.tasks.controllers;

import java.util.List;
import java.util.UUID;
import javax.inject.Inject;
import org.cqrs.Repository;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.RestController;
import org.tasksmanager.Repositories.ToDoTaskDao;

@RestController
@RequestMapping("/api/tasks/todo")
public class ToDoTasksController {

    private final Repository<ToDoTaskDao,UUID> _repository;

    @Inject
    public ToDoTasksController(Repository<ToDoTaskDao,UUID> toDoTasksRepository) {
        _repository = toDoTasksRepository;
    }

    @ResponseBody
    @RequestMapping(
            value = "/{id}",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE)
    public ToDoTaskDao GetById(@PathVariable("id") UUID id) {
        return _repository.GetById(id);
    }

    @ResponseBody
    @RequestMapping(
            value = "",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE)
    public List<ToDoTaskDao> GetAll() {
        return _repository.GetAll();
    }
}
