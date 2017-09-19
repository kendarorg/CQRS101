package controllers;

import java.util.List;
import java.util.UUID;
import java.util.concurrent.atomic.AtomicLong;
import org.cqrs.Repository;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.RestController;
import org.tasksmanager.Repositories.ToDoTaskDao;

@RestController
@RequestMapping("/api/tasks/todo")
public class ToDoTasksController {

    private Repository<ToDoTaskDao> _repository;

    public ToDoTasksController(Repository<ToDoTaskDao> repository) {
        _repository = repository;
    }

    @ResponseBody
    @RequestMapping(
            value = "/{id}",
            method = RequestMethod.GET,
            produces = MediaType.APPLICATION_JSON_VALUE,
            consumes = MediaType.APPLICATION_JSON_VALUE)
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