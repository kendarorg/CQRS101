using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;

// ReSharper disable RedundantLambdaSignatureParentheses
namespace InMemory.Scheduler
{
    [TestClass]
    public class InMemorySchedulerTest
    {
        private InMemoryScheduler _target;
        private List<DateTime> _times;

        private void VerifyRunForId(DateTime now, int id)
        {
            var result = _times[id];
            Assert.AreEqual(
                now.ToString(CultureInfo.InvariantCulture), 
                result.ToString(CultureInfo.InvariantCulture));
            Assert.AreNotSame(now, result);
        }

        [TestInitialize]
        public void Setup()
        {
            _target = new InMemoryScheduler();
            _times = new List<DateTime>();
        }

        [TestMethod]
        public void ShouldRunScheduledEvents()
        {
            //Given
            _target.Register(TimeSpan.MaxValue, _times.Add);
            var now = DateTime.UtcNow;

            //When
            _target.ForceRun(now);

            //Then
            Assert.AreEqual(1, _times.Count);
            VerifyRunForId(now, 0);
        }

        [TestMethod]
        public void ShouldIgnoreException()
        {
            //Given
            _target.Register(TimeSpan.MaxValue, (a) => throw new Exception());
            var now = DateTime.UtcNow;

            //When
            _target.ForceRun(now);

            //Then
            Assert.AreEqual(0, _times.Count);
        }

        [TestMethod]
        public void ShouldRunAllScheduledEvents()
        {
            //Given
            _target.Register(TimeSpan.MaxValue, _times.Add);
            _target.Register(TimeSpan.MaxValue, _times.Add);
            var now = DateTime.UtcNow;

            //When
            _target.ForceRun(now);

            //Then
            Assert.AreEqual(2, _times.Count);
            VerifyRunForId(now, 0);
            VerifyRunForId(now, 1);
            Assert.AreNotSame(_times[0], _times[1]);
        }
    }
}
