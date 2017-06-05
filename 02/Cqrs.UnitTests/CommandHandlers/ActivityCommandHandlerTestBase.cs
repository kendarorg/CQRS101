using Commons;
using Cqrs.ActivityDayContext;
using Cqrs.Commons;
using Cqrs.LogContext.Commands;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManager.Implementation.ActivityContext.Repositories.Entities;
using TasksManager.SharedContext.VOs;

namespace Cqrs.UnitTests.CommandHandlers
{
    public class ActivityCommandHandlerTestBase
    {
        protected ActivityCommandHandler _target;
        protected Mock<IRepository<ActivityDay, int>> _repository;
        protected Mock<IBus> _eventBus;
        protected Mock<IValidatorService> _validatorService;

        protected List<ActivityDay> _savedDays = new List<ActivityDay>();
        protected List<IMessage> _messages = new List<IMessage>();
        protected Mock<IActivityTypesService> _activityTypesService;

        [SetUp]
        public void SetUp()
        {
            _savedDays.Clear();
            _messages.Clear();

            _repository = new Mock<IRepository<ActivityDay, int>>();
            _eventBus = new Mock<IBus>();
            _validatorService = new Mock<IValidatorService>();
            _activityTypesService = new Mock<IActivityTypesService>();
            _target = new ActivityCommandHandler(
                _repository.Object, _eventBus.Object,
                _validatorService.Object,
                _activityTypesService.Object);

            _repository.Setup(a => a.Save(It.IsAny<ActivityDay>())).
                Callback<ActivityDay>(a => _savedDays.Add(a));
            _repository.Setup(a => a.Update(It.IsAny<ActivityDay>())).
                Callback<ActivityDay>(a => _savedDays.Add(a));
            _eventBus.Setup(a => a.SendAsync(It.IsAny<IMessage>())).
                Callback<IMessage>(a => _messages.Add(a));
            _eventBus.Setup(a => a.SendSync(It.IsAny<IMessage>())).
                Callback<IMessage>(a => _messages.Add(a));

        }

        protected void AssertThatNoActionHadBeenDone()
        {
            _repository.Verify((a) => a.GetById(It.IsAny<int>()), Times.Never);
            _repository.Verify((a) => a.Save(It.IsAny<ActivityDay>()), Times.Never);
            _eventBus.Verify((a) => a.SendAsync(It.IsAny<IMessage>()), Times.Never);
            _eventBus.Verify((a) => a.SendSync(It.IsAny<IMessage>()), Times.Never);
        }
    }
}
