using System;
using System.Linq;
using Commons;
using Cqrs.Commons;
using Cqrs.LogContext.Commands;
using TasksManager.Implementation.ActivityContext.Repositories.Entities;
using TasksManager.SharedContext.Events;
using TasksManager.ActivityContext.Commands;
using TasksManager.SharedContext.VOs;

namespace Cqrs.ActivityDayContext
{
    public class ActivityCommandHandler :
        ICommandHandler<CreateActivity>,
        ICommandHandler<CompleteActivity>
    {
        private readonly IRepository<ActivityDay, int> _repository;
        private readonly IBus _eventBus;
        private IValidatorService _validatorService;
        private IActivityTypesService _activityTypes;
        private IUsersService _usersService;

        public ActivityCommandHandler(
            IRepository<ActivityDay, int> repository,
            IBus eventBus,
            IValidatorService validatorService,
            IActivityTypesService activityTypes,
            IUsersService usersService)
        {
            _usersService = usersService;
            _validatorService = validatorService;
            _activityTypes = activityTypes;
            _repository = repository;
            _eventBus = eventBus;
        }

        #region Command Handler

        public void Handle(CreateActivity command)
        {
            _validatorService.Validate(command);
            var task = _activityTypes.GetByCode(command.ActivityTypeCode);
            var intDay = GetDayInt(command.From.Value);
            var day = _repository.GetById(intDay);
            var companyId = _usersService.GetUserCompany(command.UserId);
            if (day == null)
            {
                day = new ActivityDay
                {
                    CompanyId = companyId,
                    UserId = command.UserId,
                    Day = intDay
                };
            }
            var id = Guid.NewGuid();
            day.Activities.Add(new Activity
            {
                Id = id,
                From = command.From.Value,
                Description = command.Description,
                TypeCode = task.Code,
                TypeName = task.Name
            });
            _repository.Save(day);
            _eventBus.SendAsync(new ActivityCreated
            {
                UserId = command.UserId,
                CompanyId = companyId,
                Day = day.Day,
                Description = command.Description,
                From = command.From.Value,
                Id = id
            });
        }

        public void Handle(ModifyActivity command)
        {
            _validatorService.Validate(command);
            var task = _activityTypes.GetByCode(command.ActivityTypeCode);
            var day = _repository.GetById(command.Day);
            var activity = day.Activities.First(a => a.Id == command.Id);
            activity.TypeCode = task.Code;
            activity.TypeName = task.Name;
            _repository.Save(day);
            var companyId = _usersService.GetUserCompany(command.UserId);
            _eventBus.SendAsync(new ActivityModified
            {
                UserId = command.UserId,
                CompanyId = companyId,
                Day = day.Day,
                TypeCode = task.Code,
                TypeName = task.Name,
                Id = activity.Id
            });
        }

        public void Handle(CompleteActivity command)
        {
            _validatorService.Validate(command);
            var day = _repository.GetById(command.Day);
            var activity = day.Activities.
                First(a => a.Id == command.Id);
            activity.To = command.To;
            _repository.Update(day);
            var companyId = _usersService.GetUserCompany(command.UserId);
            _eventBus.SendAsync(new ActivityCompleted
            {
                UserId = command.UserId,
                CompanyId = companyId,
                Day = day.Day,
                Description = activity.Description,
                From = activity.From,
                To = activity.To.Value,
                Id = activity.Id
            });
        }

        #endregion Command Handler

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
