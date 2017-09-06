using System.Linq;
using Cqrs.Commons;
using Cqrs.LogContext.Commands;
using Cqrs.UnitTests.CommandHandlers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TasksManager.Implementation.ActivityContext.Repositories.Entities;
using TasksManager.ViewsContext.Projections.Entities;
using TasksManager.SharedContext.Events;

namespace Cqrs.UnitTests.CommandHandlers
{
    [TestFixture]
    [Category("ActivityCommandHandler")]
    public class CompleteActivityTest : ActivityCommandHandlerTestBase
    {
        [Test]
        public void ShouldStopCompleteActiviyWhenInvalid()
        {
            var command = new CompleteActivity();
            var exception = new Exception();
            _completeValidator.Setup(a => a.Validate(It.IsAny<CompleteActivity>())).
                Throws(exception);

            var result = Assert.Throws<Exception>(() => _target.Handle(command));
            Assert.AreSame(exception, result);
            AssertThatNoActionHadBeenDone();
        }

        [Test]
        public void ShouldSendEventMessage()
        {
            var day = 1;
            var id = Guid.NewGuid();
            var from = DateTime.UtcNow;
            var to = DateTime.UtcNow + TimeSpan.FromDays(1);
            CompleteActivity command = InitializeCommand(day, id, from, to);

            _target.Handle(command);

            _eventBus.Verify((a) => a.SendAsync(It.IsAny<IMessage>()), Times.Once);
            Assert.AreEqual(1, _messages.Count);
            var message = _messages.First();
            Assert.IsInstanceOf<ActivityCompleted>(message);
        }

        [Test]
        public void ShouldUpdateActivity()
        {
            var day = 1;
            var id = Guid.NewGuid();
            var from = DateTime.UtcNow;
            var to = DateTime.UtcNow + TimeSpan.FromDays(1);
            CompleteActivity command = InitializeCommand(day, id, from, to);

            _target.Handle(command);

            _repository.Verify((a) => a.Update(It.IsAny<ActivityDay>()), Times.Once);
            Assert.AreEqual(1, _savedDays.Count);
            var savedDay = _savedDays.First();
            Assert.AreEqual(1, savedDay.Activities.Count);
            Assert.IsNotNull(savedDay.Activities.First().To);
        }

        private CompleteActivity InitializeCommand(int day, Guid id, DateTime from, DateTime to)
        {
            var activityToComplete = new ActivityDay
            {
                Day = day,
                Activities = new List<Activity>
                {
                    new Activity
                    {
                        From =from,
                        Id = id
                    }
                }
            };
            var command = new CompleteActivity
            {
                Day = day,
                Id = id,
                To = to
            };

            _repository.Setup(a => a.GetById(It.Is<int>(i => i == day))).
                Returns(activityToComplete);
            return command;
        }
    }
}
