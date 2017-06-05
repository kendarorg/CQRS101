using Moq;
using NUnit.Framework;
using TasksManager.Implementation.ActivityContext.Validators;
using TasksManager.SharedContext.VOs;

namespace Cqrs.UnitTests.Validators
{
    [TestFixture]
    [Category("Complete and Modify Activity With Type")]
    public class ValidatorWithTypeTest
    {
        private Mock<IActivityTypesService> _activityTypes;
        private ActivityValidatorWithType _target;

        [SetUp]
        public void SetUp()
        {
            _activityTypes = new Mock<IActivityTypesService>();
            _target = new ActivityValidatorWithType(_activityTypes.Object);
        }

        [Test]
        public void ShouldBlockInvalidTypeCodesOnCreation() { }
        [Test]
        public void ShouldBlockInvalidTypeCodesOnModification() { }
        [Test]
        public void ShouldAllowValidTypeCodesOnCreation() { }
        [Test]
        public void ShouldAllowValidTypeCodesOnModification() { }
        [Test]
        public void ShouldIgnoreNullCommandsOnCreation() { }
        [Test]
        public void ShouldIgnoreNullCommandsOnModification() { }
    }
}
