using Cqrs;
using System;
using System.Collections.Generic;
using System.Web.Http;
using TasksManager.Repositories;

namespace TasksManager.Web.Controllers
{
    [RoutePrefix("api/tasks/stats")]
    public class TaskTypeStatsController : ApiController
    {
        private IRepository<TaskTypeStatDao, String> _repository;

        public TaskTypeStatsController(IRepository<TaskTypeStatDao, String> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("{id}")]
        public TaskTypeStatDao GetById(String id)
        {
            return _repository.GetById(id);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<TaskTypeStatDao> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
