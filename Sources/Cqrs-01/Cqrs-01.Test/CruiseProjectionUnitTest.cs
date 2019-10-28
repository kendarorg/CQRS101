using System;
using Cruise;
using Cruise.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtils;

namespace Cqrs_01.Test
{
    [TestClass]
    public class CruiseProjectionUnitTest: BaseTestEntity<RoomsForTripsEntity>
    {
        private RoomsForTripsProjection _target;


        [TestInitialize]
        public void Setup()
        {
            Initialize();
            _target = new RoomsForTripsProjection(_bus.Object,_repository.Object);
        }


        [TestMethod]
        public void ShouldAddRoomWhenNotifiedOfSuccess()
        {
            //Given
            var message = new RoomCreated(
                Guid.NewGuid(),
                1, 2, 3,
                true, Guid.NewGuid());

            //When
            _target.Handle(message);
            
            //Then
            Assert.AreEqual(0,_messages.Count);
            Assert.AreEqual(1, _saved.Count);
            Assert.AreEqual(2,_saved.Class);
        }


        [TestMethod]
        public void ShouldIgnoreFailMessages()
        {
            //Given
            var message = new RoomCreated(
                Guid.NewGuid(),
                1, 2, 3,
                false, Guid.NewGuid());

            //When
            _target.Handle(message);
            
            Assert.AreEqual(0,_messages.Count);
            Assert.IsNull(_saved);
        }

        [TestMethod]
        public void ShouldIncrementRoomWhenNotifiedOfSuccess()
        {
            //Given
            var message = new RoomCreated(
                Guid.NewGuid(),
                1, 2, 3,
                true, Guid.NewGuid());
            _saved = new RoomsForTripsEntity
            {
                CruiseId = message.CruiseId,
                Count = 1,
                Class = 2,
                Id = Guid.NewGuid()
            };

            //When
            _target.Handle(message);
            
            Assert.AreEqual(0,_messages.Count);
            
            Assert.AreEqual(2,_saved.Count);
            Assert.AreEqual(2, _saved.Class);
        }
    }
}