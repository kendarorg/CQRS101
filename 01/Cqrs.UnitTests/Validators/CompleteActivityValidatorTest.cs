using Commons;
using Cqrs.LogContext.Commands;
using Moq;
using NUnit.Framework;
using System;
using TasksManager.Implementation.ActivityContext.Validators;
using TasksManager.ViewsContext.Projections.Entities;

namespace Cqrs.UnitTests.Validators
{
    [TestFixture]
    [Category("CompleteActivityValidator")]
    public class CompleteActivityValidatorTest
    {
        private Mock<IRepository<NotCompletedActivity, Guid>> _repository;
        private CompleteActivityValidator _target;

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IRepository<NotCompletedActivity, Guid>>();
            _target = new CompleteActivityValidator(_repository.Object);
        }

        [Test]
        public void ShouldNotAllowNotFindingItem()
        {
            var command = new CompleteActivity { Id = Guid.NewGuid() };

            Assert.Throws<Exception>(() => _target.Validate(command), "Missing activity");
            _repository.Verify(a => a.GetById(It.Is<Guid>(b => b == command.Id)), Times.Once);
        }

        [Test]
        public void ShouldNotAllowNotCorrespondingDates()
        {
            var from = DateTime.UtcNow;
            var to = from - TimeSpan.FromDays(1);
            var id = Guid.NewGuid();
            var activityToComplete = new NotCompletedActivity
            {
                From = from
            };
            var command = new CompleteActivity
            {
                Id = id,
                To = to
            };

            _repository.Setup(a => a.GetById(It.Is<Guid>(b => b == command.Id))).
                Returns(activityToComplete);

            Assert.Throws<Exception>(() => _target.Validate(command), "Cannot terminate before beginning");
            _repository.Verify(a => a.GetById(It.Is<Guid>(b => b == command.Id)), Times.Once);
        }

        [Test]
        public void ShouldNotAllowNotCorrespondingDayKey()
        {
            var from = DateTime.UtcNow;
            var to = from + TimeSpan.FromDays(1);
            var id = Guid.NewGuid();
            var commandDay = 1;
            var storageDay = 2;
            var activityToComplete = new NotCompletedActivity
            {
                From = from,
                Day = storageDay
            };
            var command = new CompleteActivity
            {
                Id = id,
                To = to,
                Day = commandDay
            };

            _repository.Setup(a => a.GetById(It.Is<Guid>(b => b == command.Id))).
                Returns(activityToComplete);

            Assert.Throws<Exception>(() => _target.Validate(command), "Activity in different day");
            _repository.Verify(a => a.GetById(It.Is<Guid>(b => b == command.Id)), Times.Once);
        }


        [Test]
        public void ShouldNotAllowUnsetDay()
        {
            var from = DateTime.UtcNow;
            var to = from + TimeSpan.FromDays(1);
            var id = Guid.NewGuid();
            var commandDay = 0;
            var storageDay = 2;
            var activityToComplete = new NotCompletedActivity
            {
                From = from,
                Day = storageDay
            };
            var command = new CompleteActivity
            {
                Id = id,
                To = to,
                Day = commandDay
            };

            _repository.Setup(a => a.GetById(It.Is<Guid>(b => b == command.Id))).
                Returns(activityToComplete);

            Assert.Throws<Exception>(() => _target.Validate(command), "Day not set");
            _repository.Verify(a => a.GetById(It.Is<Guid>(b => b == command.Id)), Times.Once);
        }


        [Test]
        public void ShouldAllowCorrectData()
        {
            var from = DateTime.UtcNow;
            var to = from + TimeSpan.FromDays(1);
            var id = Guid.NewGuid();
            var commandDay = 2;
            var storageDay = 2;
            var activityToComplete = new NotCompletedActivity
            {
                From = from,
                Day = storageDay
            };
            var command = new CompleteActivity
            {
                Id = id,
                To = to,
                Day = commandDay
            };

            _repository.Setup(a => a.GetById(It.Is<Guid>(b => b == command.Id))).
                Returns(activityToComplete);

            _target.Validate(command);

            _repository.Verify(a => a.GetById(It.Is<Guid>(b => b == command.Id)), Times.Once);
        }
    }
}
