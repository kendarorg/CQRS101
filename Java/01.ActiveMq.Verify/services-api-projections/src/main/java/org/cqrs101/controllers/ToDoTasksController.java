package org.cqrs101.controllers;

import java.util.List;
import java.util.UUID;
import javax.inject.Inject;
import org.cqrs101.Repository;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.RestController;
import org.cqrs101.views.repositories.ToDoTaskDao;

@RestController
@RequestMapping("/api/tasks/todo")
public class ToDoTasksController {

    private final Repository<ToDoTaskDao> repository;

    @Inject
    public ToDoTasksController(Repository<ToDoTaskDao> toDoTasksRepository) {
        this.repository = toDoTasksRepository;
    }

    @ResponseBody
    @RequestMapping(
            value = "/{id}",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE,
            consumes = MediaType.APPLICATION_JSON_VALUE)
    public ToDoTaskDao getById(@PathVariable("id") UUID id) {
        return repository.getById(id);
    }

    @ResponseBody
    @RequestMapping(
            value = "",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE)
    public List<ToDoTaskDao> getAll() {
        return repository.getAll();
    }
}
