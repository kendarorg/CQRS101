package org.cqrs101.controllers;

import java.util.List;
import java.util.UUID;
import javax.inject.Inject;
import org.cqrs101.Repository;
import org.cqrs101.views.repositories.InProgressInvoice;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/invoices/inprogress")
public class InProgressInvoicesController {

    private final Repository<InProgressInvoice> repository;

    @Inject
    public InProgressInvoicesController(Repository<InProgressInvoice> toDoTasksRepository) {
        this.repository = toDoTasksRepository;
    }

    @ResponseBody
    @RequestMapping(
            value = "/{id}",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE,
            consumes = MediaType.APPLICATION_JSON_VALUE)
    public InProgressInvoice getById(@PathVariable("id") UUID id) {
        return repository.getById(id);
    }

    @ResponseBody
    @RequestMapping(
            value = "",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE)
    public List<InProgressInvoice> getAll() {
        return repository.getAll();
    }
}
