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

namespace Cqrs.UnitTests.CommandHandlers
{
    public class ActivityCommandHandlerTestBase
    {
        protected ActivityCommandHandler _target;
        protected Mock<IRepository<ActivityDay, int>> _repository;
        protected Mock<IBus> _eventBus;
        protected Mock<IValidator<CreateActivity>> _createValidator;
        protected Mock<IValidator<CompleteActivity>> _completeValidator;

        protected List<ActivityDay> _savedDays = new List<ActivityDay>();
        protected List<IMessage> _messages = new List<IMessage>();


        [SetUp]
        public void SetUp()
        {
            _savedDays.Clear();
            _messages.Clear();

            _repository = new Mock<IRepository<ActivityDay, int>>();
            _eventBus = new Mock<IBus>();
            _createValidator = new Mock<IValidator<CreateActivity>>();
            _completeValidator = new Mock<IValidator<CompleteActivity>>();
            _target = new ActivityCommandHandler(
                _repository.Object, _eventBus.Object,
                _createValidator.Object, _completeValidator.Object);

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
