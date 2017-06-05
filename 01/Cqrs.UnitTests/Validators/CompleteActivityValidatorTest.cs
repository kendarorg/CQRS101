using Commons;
using Cqrs.LogContext.Commands;
using Moq;
using NUnit.Framework;
using System;
using TasksManager.Implementation.ActivityContext.Repositories.Entities;
using TasksManager.Implementation.ActivityContext.Validators;
using TasksManager.ViewsContext.Projections.Entities;

namespace Cqrs.UnitTests.Validators
{
    [TestFixture]
    [Category("ValidatorCompleteActivityTest")]
    public class ValidatorCompleteActivityTest
    {
        private Mock<IRepository<NotCompletedActivity, Guid>> _notCompletedRepository;
        private CompleteActivityValidator _target;
        private Mock<IRepository<ActivityDay, int>> _activityDays;

        [SetUp]
        public void SetUp()
        {
            _notCompletedRepository = new Mock<IRepository<NotCompletedActivity, Guid>>();
            _activityDays = new Mock<IRepository<ActivityDay, int>>();
            _target = new CompleteActivityValidator(_notCompletedRepository.Object, _activityDays.Object);

            _activityDays.Setup(a => a.GetById(It.IsAny<int>())).Returns(new ActivityDay());
        }

        [Test]
        public void ShouldNotAllowNotFindingItem()
        {
            var command = new CompleteActivity { Id = Guid.NewGuid() };

            Assert.Throws<Exception>(() => _target.Validate(command), "Missing activity");
            _notCompletedRepository.Verify(a => a.GetById(It.Is<Guid>(b => b == command.Id)), Times.Once);
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

            _notCompletedRepository.Setup(a => a.GetById(It.Is<Guid>(b => b == command.Id))).
                Returns(activityToComplete);

            Assert.Throws<Exception>(() => _target.Validate(command), "Cannot terminate before beginning");
            _notCompletedRepository.Verify(a => a.GetById(It.Is<Guid>(b => b == command.Id)), Times.Once);
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

            _notCompletedRepository.Setup(a => a.GetById(It.Is<Guid>(b => b == command.Id))).
                Returns(activityToComplete);

            Assert.Throws<Exception>(() => _target.Validate(command), "Activity in different day");
            _notCompletedRepository.Verify(a => a.GetById(It.Is<Guid>(b => b == command.Id)), Times.Once);
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

            _notCompletedRepository.Setup(a => a.GetById(It.Is<Guid>(b => b == command.Id))).
                Returns(activityToComplete);

            Assert.Throws<Exception>(() => _target.Validate(command), "Day not set");
            _notCompletedRepository.Verify(a => a.GetById(It.Is<Guid>(b => b == command.Id)), Times.Once);
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

            _notCompletedRepository.Setup(a => a.GetById(It.Is<Guid>(b => b == command.Id))).
                Returns(activityToComplete);

            _target.Validate(command);

            _notCompletedRepository.Verify(a => a.GetById(It.Is<Guid>(b => b == command.Id)), Times.Once);
        }
    }
}
