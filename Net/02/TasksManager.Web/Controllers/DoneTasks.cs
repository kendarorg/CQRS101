using Cqrs;
using System;
using System.Collections.Generic;
using System.Web.Http;
using TasksManager.Repositories;

namespace TasksManager.Web.Controllers
{
    [RoutePrefix("api/tasks/done")]
    public class DoneTasksController : ApiController
    {
        private IRepository<DoneTaskDao, Guid> _repository;

        public DoneTasksController(IRepository<DoneTaskDao, Guid> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("{id}")]
        public DoneTaskDao GetById(Guid id)
        {
            return _repository.GetById(id);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<DoneTaskDao> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
