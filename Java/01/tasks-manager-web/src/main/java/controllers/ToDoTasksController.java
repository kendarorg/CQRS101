package controllers;

import java.util.concurrent.atomic.AtomicLong;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/tasks/todo")
public class ToDoTasksController {
    /*private Repository<ToDoTaskDao> _repository;

        public ToDoTasksController(Repository<ToDoTaskDao> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("{id}")]
        public ToDoTaskDao GetById(Guid id)
        {
            return _repository.GetById(id);
        }

        [HttpGet]
        [Route("")]
        
        public IEnumerable<ToDoTaskDao> GetAll()
        {
            return _repository.GetAll();
        }*/
}
