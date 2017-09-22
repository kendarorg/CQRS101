package org.cqrs101.controllers;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.logging.Logger;
import javax.inject.Inject;
import org.cqrs101.Repository;
import org.cqrs101.shared.services.TaskServiceDao;
import org.cqrs101.tasks.repositories.TaskDao;
import org.springframework.beans.BeanUtils;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/tasks")
public class TasksServiceController {

    private final Repository<TaskDao> repository;
    private static final Logger logger = Logger.getLogger(TasksServiceController.class.getSimpleName());

    @Inject
    public TasksServiceController(Repository<TaskDao> tasksRepository) {
        this.repository = tasksRepository;
    }

    @ResponseBody
    @RequestMapping(
            value = "/{id}",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE)
    public TaskServiceDao getById(@PathVariable("id") UUID id) {
        TaskServiceDao result = null;
        TaskDao partialResult = repository.getById(id);
        if (partialResult != null) {
            result = new TaskServiceDao();
            BeanUtils.copyProperties(partialResult, result);
        }
        return result;
    }

    @ResponseBody
    @RequestMapping(
            value = "",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE)
    public List<TaskServiceDao> getAll() {
        List<TaskServiceDao> result = new ArrayList<>();
        List<TaskDao> partialResult = repository.getAll();
        for (int i = 0; i < partialResult.size(); i++) {
            TaskServiceDao subResult = new TaskServiceDao();
            BeanUtils.copyProperties(partialResult.get(i), subResult);
        }
        return result;
    }
}
