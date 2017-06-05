using Cqrs.Commons;
using Cqrs.LogContext.Commands;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using TasksManager.Implementation.ActivityContext.Repositories.Entities;
using TasksManager.SharedContext.Events;

namespace Cqrs.UnitTests.CommandHandlers
{
    [TestFixture]
    [Category("ActivityCommandHandler")]
    public class ActivityCommandHandlerTest : ActivityCommandHandlerTestBase
    {
        [Test]
        public void ShouldStopCreateActiviyWhenInvalid()
        {
            var command = new CreateActivity();
            var exception = new Exception();
            _createValidator.Setup(a => a.Validate(It.IsAny<CreateActivity>())).
                Throws(exception);

            var result = Assert.Throws<Exception>(() => _target.Handle(command));
            Assert.AreSame(exception, result);
            AssertThatNoActionHadBeenDone();
        }


        [Test]
        public void ShouldSaveActivityDayOnRepository()
        {
            var description = "Test";
            var from = DateTime.UtcNow;

            CreateActivity command = InitializeCommand(description, from);

            _target.Handle(command);

            _repository.Verify((a) => a.Save(It.IsAny<ActivityDay>()), Times.Once);
            Assert.AreEqual(1, _savedDays.Count);
            var savedDay = _savedDays.First();
            Assert.AreEqual(1, savedDay.Activities.Count);
        }

        [Test]
        public void ShouldSendEventOnBus()
        {
            var description = "Test";
            var from = DateTime.UtcNow;

            CreateActivity command = InitializeCommand(description, from);

            _target.Handle(command);
            
            _eventBus.Verify((a) => a.SendAsync(It.IsAny<IMessage>()), Times.Once);
            Assert.AreEqual(1, _messages.Count);
            Assert.IsInstanceOf<ActivityCreated>(_messages.First());
        }

        private static CreateActivity InitializeCommand(string description, DateTime from)
        {
            return new CreateActivity
            {
                Description = description,
                From = from
            };
        }
    }
}
