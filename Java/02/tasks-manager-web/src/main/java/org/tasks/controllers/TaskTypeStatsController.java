package org.tasks.controllers;

import java.util.List;
import javax.inject.Inject;
import org.cqrs.Repository;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.RestController;
import org.tasksmanager.Repositories.TaskTypeStatDao;

@RestController
@RequestMapping("/api/tasks/stats")
public class TaskTypeStatsController {

    private final Repository<TaskTypeStatDao,String> _repository;

    @Inject
    public TaskTypeStatsController(Repository<TaskTypeStatDao,String> doneTasksRepository) {
        _repository = doneTasksRepository;
    }

    @ResponseBody
    @RequestMapping(
            value = "/{id}",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE)
    public TaskTypeStatDao GetById(@PathVariable("id") String id) {
        return _repository.GetById(id);
    }

    @ResponseBody
    @RequestMapping(
            value = "",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE)
    public List<TaskTypeStatDao> GetAll() {
        return _repository.GetAll();
    }
}
