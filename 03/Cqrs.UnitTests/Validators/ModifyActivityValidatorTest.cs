using Commons;
using Cqrs.LogContext.Commands;
using Cqrs.SharedContext.Services;
using Cqrs.SharedContext.Services.Dtos;
using Moq;
using NUnit.Framework;
using System;
using TasksManager.ActivityContext.Commands;
using TasksManager.Implementation.ActivityContext.Validators;
using TasksManager.ViewsContext.Projections.Entities;

namespace Cqrs.UnitTests.Validators
{
    [TestFixture]
    [Category("ModifyActivityValidator")]
    public class ModifyActivityValidatorTest
    {
        private ModifyActivityValidator _target;
        private Mock<IActivityTypesService> _typesService;
        private Mock<IRepository<NotCompletedActivity, Guid>> _repository;

        [SetUp]
        public void SetUp()
        {
            _typesService = new Mock<IActivityTypesService>();
            _repository = new Mock<IRepository<NotCompletedActivity, Guid>>();
            _target = new ModifyActivityValidator(_repository.Object, _typesService.Object);
        }

        [Test]
        [TestCase("", "test")]
        [TestCase(" ", "test")]
        [TestCase(null, "test")]
        public void ShouldAllowEmptyDescriptionWhenCodeExists(string description, string code)
        {
            var activity = new NotCompletedActivity
            {
                Id = Guid.NewGuid()
            };
            var from = DateTime.UtcNow;
            var activityType = new ActivityTypeDto
            {
                Code = code,
            };
            var command = new ModifyActivity
            {
                Id = activity.Id,
                ActivityTypeCode = code,
                Description = description
            };
            _repository.Setup(a => a.GetById(It.Is<Guid>(g => g == activity.Id))).
                Returns(activity);
            _typesService.Setup(a => a.GetByCode(It.Is<String>(s => s == code))).
                Returns(activityType);

            _target.Validate(command);
        }

        [Test]
        [TestCase("test", "")]
        [TestCase("test", " ")]
        [TestCase("test", null)]
        public void ShouldAllowEmptyCodeWhenDescriptionExists(string description, string code)
        {
            var activity = new NotCompletedActivity
            {
                Id = Guid.NewGuid()
            };
            var activityType = new ActivityTypeDto
            {
                Code = code,
            };
            _repository.Setup(a => a.GetById(It.Is<Guid>(g => g == activity.Id))).
                Returns(activity);

            var command = new ModifyActivity
            {
                Id = activity.Id,
                ActivityTypeCode = code,
                Description = description
            };

            _target.Validate(command);
        }

        [Test]
        [TestCase("  ", "")]
        [TestCase(null, " ")]
        [TestCase("", null)]
        public void ShouldNotAllowNullDescriptionAndCode(string description, string code)
        {
            var activity = new NotCompletedActivity
            {
                Id = Guid.NewGuid()
            };
            var command = new ModifyActivity
            {
                Id = activity.Id,
                Description = description
            };
            _repository.Setup(a => a.GetById(It.Is<Guid>(g => g == activity.Id))).
                Returns(activity);

            Assert.Throws<Exception>(() => _target.Validate(command), "Invalid description");
        }

        [Test]
        public void ShouldNotAllowMissingActivity()
        {
            var command = new ModifyActivity
            {
                Id = Guid.NewGuid()
            };

            Assert.Throws<Exception>(() => _target.Validate(command), "Missing activity");
        }


        [Test]
        public void ShouldAllowCorrectData()
        {
            string description = "test";
            string code = "code";
            var activity = new NotCompletedActivity
            {
                Id = Guid.NewGuid()
            };
            var activityType = new ActivityTypeDto
            {
                Code = code,
            };
            _repository.Setup(a => a.GetById(It.Is<Guid>(g => g == activity.Id))).
                Returns(activity);

            var command = new ModifyActivity
            {
                Id = activity.Id,
                ActivityTypeCode = code,
                Description = description
            };

            _target.Validate(command);
        }
    }
}
