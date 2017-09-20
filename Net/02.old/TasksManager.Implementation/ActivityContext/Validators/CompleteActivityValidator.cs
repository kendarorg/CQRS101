using Commons;
using Cqrs.Commons;
using Cqrs.LogContext.Commands;
using System;
using System.Linq;
using TasksManager.Implementation.ActivityContext.Repositories.Entities;
using TasksManager.ViewsContext.Projections.Entities;

namespace TasksManager.Implementation.ActivityContext.Validators
{
    public class CompleteActivityValidator :
        IValidator<CompleteActivity>
    {
        private IRepository<ActivityDay, int> _repository;
        private IRepository<NotCompletedActivity, Guid> _notCompletedRepository;

        public CompleteActivityValidator(
            IRepository<NotCompletedActivity, Guid> notCompletedRepository,
            IRepository<ActivityDay, int> activityDaysRepository)
        {
            _notCompletedRepository = notCompletedRepository;
            _repository = activityDaysRepository;
        }

        public bool Validate(CompleteActivity item)
        {
            var toComplete = _notCompletedRepository.GetById(item.Id);
            if (toComplete == null) throw new Exception("Missing activity");
            if (toComplete.From >= item.To) throw new Exception("Cannot terminate before beginning");
            if (toComplete.Day != item.Day) throw new Exception("Activity in different day");
            if (0 == item.Day) throw new Exception("Day not set");
            
            //Assumed presence of the activity day
            var activityDay = _repository.GetById(item.Day);
            var totalTicks = activityDay.Activities.
                Where(a => a.To != null).
                Sum(a => (a.To.Value - a.From).Ticks);
            if (totalTicks > TimeSpan.TicksPerHour * 8) throw new Exception("8 Hours Exceeded");
            return true;
        }
        
    }
}
