using System;
using Cqrs.Commons;
using Cqrs.LogContext.Commands;
using TasksManager.ActivityContext.Commands;
using TasksManager.SharedContext.VOs;

namespace TasksManager.Implementation.ActivityContext.Validators
{
    public class ActivityValidatorWithType :
        IValidator<CreateActivity>,
        IValidator<ModifyActivity>
    {
        private IActivityTypesService _activityTypes;

        public ActivityValidatorWithType(IActivityTypesService activityTypes)
        {
            _activityTypes = activityTypes;
        }

        public bool Validate(ModifyActivity command)
        {
            if (command == null) return false;
            VerifyActivityTypeCode(command.ActivityTypeCode);
            return true;
        }

        public bool Validate(CreateActivity command)
        {
            if (command == null) return false;
            VerifyActivityTypeCode(command.ActivityTypeCode);
            return true;
        }

        private void VerifyActivityTypeCode(string code)
        {
            var activityType = _activityTypes.GetByCode(code);
            if (activityType == null)
            {
                throw new Exception("Invalid activity type");
            }
        }
    }
}
