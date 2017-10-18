package org.cqrs101.controllers;

import org.cqrs101.Repository;
import org.cqrs101.views.repositories.CompletedOrder;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.*;

import javax.inject.Inject;
import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/tasks/todo")
public class CompletedOrdersController {

    private final Repository<CompletedOrder> repository;

    @Inject
    public CompletedOrdersController(Repository<CompletedOrder> toDoTasksRepository) {
        this.repository = toDoTasksRepository;
    }

    @ResponseBody
    @RequestMapping(
            value = "/{id}",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE,
            consumes = MediaType.APPLICATION_JSON_VALUE)
    public CompletedOrder getById(@PathVariable("id") UUID id) {
        return repository.getById(id);
    }

    @ResponseBody
    @RequestMapping(
            value = "",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE)
    public List<CompletedOrder> getAll() {
        return repository.getAll();
    }
}
