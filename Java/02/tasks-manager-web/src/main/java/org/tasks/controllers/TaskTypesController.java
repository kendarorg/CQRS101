package org.tasks.controllers;

import java.util.List;
import javax.inject.Inject;
import org.commons.Services.TaskTypeDao;
import org.cqrs.Repository;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/taskTypes")
public class TaskTypesController {

    private final Repository<TaskTypeDao, String> _repository;

    @Inject
    public TaskTypesController(Repository<TaskTypeDao, String> taskTypesRepository) {
        _repository = taskTypesRepository;
    }
    
    @ResponseBody
    @RequestMapping(
            value = "/{id}",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE)
    public TaskTypeDao GetById(@PathVariable("id") String id) {
        return _repository.GetById(id);
    }

    @ResponseBody
    @RequestMapping(
            value = "",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE)
    public List<TaskTypeDao> GetAll() {
        return _repository.GetAll();
    }

    @ResponseBody
    @RequestMapping(
            value = "",
            method = RequestMethod.POST,
            produces = MediaType.APPLICATION_JSON_VALUE,
            consumes = MediaType.APPLICATION_JSON_VALUE)
    public TaskTypeDao Create(@RequestBody TaskTypeDao toCreate) {
        return _repository.Save(toCreate);
    }

    @ResponseBody
    @RequestMapping(
            value = "/{id}",
            method = RequestMethod.DELETE)
    public void Delete(@PathVariable("id") String id) {
        _repository.Delete(id);
    }
}
