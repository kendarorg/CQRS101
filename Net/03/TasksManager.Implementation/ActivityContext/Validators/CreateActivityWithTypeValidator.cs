using Cqrs.Commons;
using Cqrs.LogContext.Commands;
using Cqrs.SharedContext.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManager.Implementation.ActivityContext.Validators
{
    public class CreateActivityWithTypeValidator
        : IValidator<CreateActivity>
    {
        private IActivityTypesService _activityTypes;

        public CreateActivityWithTypeValidator(IActivityTypesService activityTypes)
        {
            _activityTypes = activityTypes;
        }

        public bool Validate(CreateActivity command)
        {
            var foundedTask = _activityTypes.GetByCode(command.ActivityTypeCode);
            if (foundedTask == null)
            {
                throw new Exception("Missing activity type");
            }
            return true;
        }
    }
}
