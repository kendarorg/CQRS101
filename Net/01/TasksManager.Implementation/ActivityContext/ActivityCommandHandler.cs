using System;
using System.Linq;
using Commons;
using Cqrs.Commons;
using Cqrs.LogContext.Commands;
using TasksManager.Implementation.ActivityContext.Repositories.Entities;
using TasksManager.SharedContext.Events;

namespace Cqrs.ActivityDayContext
{
    public class ActivityCommandHandler :
        ICommandHandler<CreateActivity>,
        ICommandHandler<CompleteActivity>
    {
        private readonly IRepository<ActivityDay, int> _repository;
        private readonly IBus _eventBus;
        private IValidator<CreateActivity> _createValidator;
        private IValidator<CompleteActivity> _completeValidator;

        public ActivityCommandHandler(
            IRepository<ActivityDay, int> repository,
            IBus eventBus,
            IValidator<CreateActivity> createValidator,
            IValidator<CompleteActivity> completeValidator)
        {
            _createValidator = createValidator;
            _completeValidator = completeValidator;
            _repository = repository;
            _eventBus = eventBus;
        }

        #region Command Handler

        public void Handle(CreateActivity command)
        {
            _createValidator.Validate(command);
            var intDay = GetDayInt(command.From.Value);
            var day = _repository.GetById(intDay);
            if (day == null)
            {
                day = new ActivityDay
                {
                    Day = intDay
                };
            }
            var id = Guid.NewGuid();
            day.Activities.Add(new Activity
            {
                Id = id,
                From = command.From.Value,
                Description = command.Description
            });
            _repository.Save(day);
            _eventBus.SendAsync(new ActivityCreated
            {
                Day = day.Day,
                Description = command.Description,
                From = command.From.Value,
                Id = id
            });
        }

        public void Handle(CompleteActivity command)
        {
            _completeValidator.Validate(command);
            var day = _repository.GetById(command.Day);
            var activity = day.Activities.
                First(a => a.Id == command.Id);
            activity.To = command.To;
            _repository.Update(day);
            _eventBus.SendAsync(new ActivityCompleted
            {
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
