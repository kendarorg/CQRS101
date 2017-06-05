using Commons;
using Cqrs.Commons;
using Cqrs.LogContext.Commands;
using System;
using TasksManager.ViewsContext.Projections.Entities;

namespace TasksManager.Implementation.ActivityContext.Validators
{
    public class CompleteActivityValidator :
        IValidator<CompleteActivity>
    {
        private IRepository<NotCompletedActivity, Guid> _repository;

        public CompleteActivityValidator(IRepository<NotCompletedActivity, Guid> repository)
        {
            _repository = repository;
        }

        public bool Validate(CompleteActivity item)
        {
            var toComplete = _repository.GetById(item.Id);
            if (toComplete == null) throw new Exception("Missing activity");
            if (toComplete.From >= item.To) throw new Exception("Cannot terminate before beginning");
            if (toComplete.Day != item.Day) throw new Exception("Activity in different day");
            if (0 == item.Day) throw new Exception("Day not set");
            return true;
        }
    }
}
