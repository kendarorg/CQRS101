package org.cqrs101.controllers;

import org.cqrs101.*;
import org.cqrs101.views.repositories.*;
import org.springframework.http.*;
import org.springframework.web.bind.annotation.*;

import javax.inject.*;
import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/tasks/todo")
public class CompletedInvoicesController {

    private final Repository<CompletedInvoice> repository;

    @Inject
    public CompletedInvoicesController(Repository<CompletedInvoice> toDoTasksRepository) {
        this.repository = toDoTasksRepository;
    }

    @ResponseBody
    @RequestMapping(
            value = "/{id}",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE,
            consumes = MediaType.APPLICATION_JSON_VALUE)
    public CompletedInvoice getById(@PathVariable("id") UUID id) {
        return repository.getById(id);
    }

    @ResponseBody
    @RequestMapping(
            value = "",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE)
    public List<CompletedInvoice> getAll() {
        return repository.getAll();
    }
}
