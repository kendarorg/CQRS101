using Commons;
using Commons.Repository;
using System;
using System.Collections.Generic;
using System.Web.Http;
using TasksManager.ViewsContext.Projections.Entities;

namespace Cqrs.Api.Controllers
{
    [RoutePrefix("api/views")]
    public class ViewsController : ApiController
    {
        private IRepository<CompletedActivity, Guid> _completedActivities;
        private IRepository<NotCompletedActivity, Guid> _notCompletedActivities;
        private IRepository<ActivitySummary, ActivitySummaryKey> _summary;

        public ViewsController(
            IRepository<CompletedActivity, Guid> completedActivities,
            IRepository<NotCompletedActivity, Guid> notCompletedActivities,
            IRepository<ActivitySummary, ActivitySummaryKey> summary)
        {
            _completedActivities = completedActivities;
            _notCompletedActivities = notCompletedActivities;
            _summary = summary;
        }

        [HttpPost]
        [Route("completed")]
        public IEnumerable<CompletedActivity> GetCompleted(Filter filter)
        {
            return _completedActivities.Find(filter);
        }

        [HttpPost]
        [Route("notcompleted")]
        public IEnumerable<NotCompletedActivity> GetNotCompleted(Filter filter)
        {
            return _notCompletedActivities.Find(filter);
        }
        
        [HttpPost]
        [Route("summary")]
        public IEnumerable<ActivitySummary> GetSummary(Filter filter)
        {
            return _summary.Find(filter);
        }
    }
}