using Cqrs;
using System;
using System.Collections.Generic;
using System.Web.Http;
using TasksManager.Repositories;

namespace TasksManager.Web.Controllers
{
    [RoutePrefix("api/tasks/todo")]
    public class ToDoTasksController : ApiController
    {
        private IRepository<ToDoTaskDao, Guid> _repository;

        public ToDoTasksController(IRepository<ToDoTaskDao, Guid> repository)
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
        }
    }
}
