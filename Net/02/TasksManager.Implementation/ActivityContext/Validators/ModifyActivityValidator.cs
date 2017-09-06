using Commons;
using Cqrs.Commons;
using Cqrs.LogContext.Commands;
using Cqrs.SharedContext.Services;
using System;
using TasksManager.ActivityContext.Commands;
using TasksManager.ViewsContext.Projections.Entities;

namespace TasksManager.Implementation.ActivityContext.Validators
{
    public class ModifyActivityValidator :
        IValidator<ModifyActivity>
    {
        private IActivityTypesService _activityTypes;
        private IRepository<NotCompletedActivity, Guid> _repository;

        public ModifyActivityValidator(
            IRepository<NotCompletedActivity, Guid> repository,
            IActivityTypesService activityTypes)
        {
            _repository = repository;
            _activityTypes = activityTypes;
        }

        public bool Validate(ModifyActivity item)
        {
            var toComplete = _repository.GetById(item.Id);
            if (toComplete == null) throw new Exception("Missing activity");
            var activityType = _activityTypes.GetByCode(item.ActivityTypeCode);
            if (string.IsNullOrWhiteSpace(item.Description) && activityType == null)
            {
                throw new Exception("Invalid description/type");
            }
            return true;
        }
    }
}
