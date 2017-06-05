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
    [Category("CreateActivityValidator")]
    public class ValidatorCreateActivityTest
    {
        private CreateActivityValidator _target;

        [SetUp]
        public void SetUp()
        {
            _target = new CreateActivityValidator();
        }

        [Test]
        public void ShouldNotAllowEmptyFrom()
        {
            DateTime? from = null;
            var command = new CreateActivity { From = from };

            Assert.Throws<Exception>(() => _target.Validate(command), "Invalid date");
        }

        [Test]
        public void ShouldNotAllowEmptyDescription()
        {
            var from = DateTime.UtcNow;
            var description = "";
            var command = new CreateActivity
            {
                From = from,
                Description = description
            };

            Assert.Throws<Exception>(() => _target.Validate(command), "Invalid description");
        }

        [Test]
        public void ShouldNotAllowNullDescription()
        {
            var from = DateTime.UtcNow;
            string description = null;
            var command = new CreateActivity
            {
                From = from,
                Description = description
            };

            Assert.Throws<Exception>(() => _target.Validate(command), "Invalid description");
        }

        [Test]
        public void ShouldNotAllowWhitespacesDescription()
        {
            var from = DateTime.UtcNow;
            var description = " ";
            var command = new CreateActivity
            {
                From = from,
                Description = description
            };

            Assert.Throws<Exception>(() => _target.Validate(command), "Invalid description");
        }

        [Test]
        public void ShouldAllowCorrectData()
        {
            var from = DateTime.UtcNow;
            var description = "test";
            var command = new CreateActivity
            {
                From = from,
                Description = description
            };
            _target.Validate(command);
        }
    }
}
