package org.cqrs101.controllers;

import java.util.List;
import java.util.UUID;
import javax.inject.Inject;
import org.cqrs101.Repository;
import org.cqrs101.cruds.Customer;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/customers")
public class CustomersController {

    private final Repository<Customer> repository;

    @Inject
    public CustomersController(Repository<Customer> toDoTasksRepository) {
        this.repository = toDoTasksRepository;
    }

    @ResponseBody
    @RequestMapping(
            value = "/{id}",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE,
            consumes = MediaType.APPLICATION_JSON_VALUE)
    public Customer getById(@PathVariable("id") UUID id) {
        return repository.getById(id);
    }

    @ResponseBody
    @RequestMapping(
            value = "",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE)
    public List<Customer> getAll() {
        return repository.getAll();
    }

    @ResponseBody
    @RequestMapping(
            value = "",
            method = RequestMethod.POST,
            produces = MediaType.APPLICATION_JSON_VALUE)
    public Customer add(@RequestBody Customer customer) {
        return repository.save(customer);
    }

    @ResponseBody
    @RequestMapping(
            value = "/{id}",
            method = RequestMethod.DELETE,
            produces = MediaType.APPLICATION_JSON_VALUE)
    public void delete(@PathVariable("id") UUID id) {
        repository.delete(id);
    }
}
