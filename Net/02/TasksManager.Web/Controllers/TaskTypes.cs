using Commons.Services;
using Cqrs;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace TasksManager.Web.Controllers
{
    [RoutePrefix("api/taskTypes")]
    public class TaskTypesController : ApiController
    {
        private IRepository<TaskTypeDao, String> _repository;

        public TaskTypesController(IRepository<TaskTypeDao, String> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("{id}")]
        public TaskTypeDao GetById(String id)
        {
            return _repository.GetById(id);
        }

        [HttpPost]
        [Route("")]
        public TaskTypeDao Save(TaskTypeDao toCreate)
        {
            return _repository.Save(toCreate);
        }

        [HttpDelete]
        [Route("{id}")]
        public void Delete(String id)
        {
            _repository.Delete(id);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<TaskTypeDao> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
