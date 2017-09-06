using System.Collections.Generic;
using System.Web.Http;
using Commons;
using Cqrs.VoContext.Repositories.Entities;

namespace Cqrs.Api.Controllers
{
    [RoutePrefix("api/task")]
    public class TasksController : ApiController
    {
        private readonly IRepository<ActivityType, string> _repository;

        public TasksController(IRepository<ActivityType, string> repository)
        {
            _repository = repository;
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<ActivityType> Get()
        {
            return _repository.Find();
        }

        [Route("{code}")]
        [HttpGet]
        public ActivityType Get(string code)
        {
            return _repository.GetById(code);
        }

        [Route("")]
        [HttpPost]
        public ActivityType Post(ActivityType value)
        {
            return _repository.Save(value);
        }

        [Route("{code}")]
        [HttpPut]
        public void Put(string code, ActivityType value)
        {
            _repository.Update(value);
        }

        [Route("{code}")]
        [HttpDelete]
        public void Delete(string code)
        {
            _repository.Delete(code);
        }
    }
}
