using Commons;
using Cqrs.Commons;
using Cqrs.LogContext.Commands;
using System;
using System.Linq;
using TasksManager.Implementation.ActivityContext.Repositories.Entities;

namespace TasksManager.Implementation.ActivityContext.Validators
{
    public class CreateActivityValidator
        : IValidator<CreateActivity>
    {
        private IRepository<ActivityDay, int> _repository;

        public CreateActivityValidator(IRepository<ActivityDay, int> repository)
        {
            _repository = repository;
        }

        public bool Validate(CreateActivity item)
        {
            if (item.From == null) throw new Exception("Invalid date");
            if (string.IsNullOrWhiteSpace(item.Description)) throw new Exception("Invalid description");
            var day = GetDayInt(item.From.Value);
            var activityDay = _repository.GetById(day);
            if (activityDay != null)
            {
                var totalTicks = activityDay.Activities.
                    Where(a => a.To != null).
                    Sum(a => (a.To.Value - a.From).Ticks);
                if (totalTicks > TimeSpan.TicksPerHour * 8) throw new Exception("8 Hours Exceeded");
            }
            return true;
        }

        #region Utility Methods

        private static int GetDayInt(DateTime date)
        {
            return date.Day +
                    date.Month * 100 +
                    date.Year * 100 * 100;

        }

        #endregion Utility Methods
    }
}
