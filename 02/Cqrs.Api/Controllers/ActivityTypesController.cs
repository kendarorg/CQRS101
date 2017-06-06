using Commons;
using Commons.Repository;
using Cqrs.VoContext.Repositories.Entities;
using System.Collections.Generic;
using System.Web.Http;

namespace Cqrs.Api.Controllers
{
    [RoutePrefix("api/activityTypes")]
    public class ActivityTypesController: ApiController
    {
        private IRepository<ActivityType, string> _activityTypes;

        public ActivityTypesController(IRepository<ActivityType, string> activityTypes)
        {
            _activityTypes = activityTypes;
        }

        [HttpGet]
        [Route("{code}")]
        public ActivityType GetById(string code)
        {
            return _activityTypes.GetById(code);
        }

        [HttpPost]
        [Route("find")]
        public IEnumerable<ActivityType> GetById(Filter filter)
        {
            return _activityTypes.Find(filter);
        }

        [HttpPut]
        [Route("{code}")]
        public bool Update(string code, ActivityType activity)
        {
            return _activityTypes.Update(activity);
        }

        [HttpPost]
        public ActivityType Save(ActivityType activity)
        {
            return _activityTypes.Save(activity);
        }

        [HttpDelete]
        [Route("{code}")]
        public void Delete(string code)
        {
            _activityTypes.Delete(code);
        }
    }
}