using System;
using System.Threading.Tasks;
using Cruise;
using Cruise.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NServiceBus;
using TestUtils;

namespace Cqrs_01.Test
{
    [TestClass]
    public class CruiseProjectionUnitTest: BaseTestEntity<RoomsForTripsEntity>
    {
        private RoomsForTripsProjection _target;
        private Mock<IMessageHandlerContext> _messageHandlerContext;

        [TestInitialize]
        public void Setup()
        {
            Initialize();
            _target = new RoomsForTripsProjection(_repository.Object);
            _messageHandlerContext = new Mock<IMessageHandlerContext>();
            _messageHandlerContext.Setup(a => a.Publish(It.IsAny<object>(), It.IsAny<PublishOptions>()))
                .Returns((object ob, PublishOptions po) =>
                {
                    _messages.Add(ob);
                    return Task.CompletedTask;
                });
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
            _target.Handle(message, _messageHandlerContext.Object);
            
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
            _target.Handle(message, _messageHandlerContext.Object);
            
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
            _target.Handle(message, _messageHandlerContext.Object);
            
            Assert.AreEqual(0,_messages.Count);
            
            Assert.AreEqual(2,_saved.Count);
            Assert.AreEqual(2, _saved.Class);
        }
    }
}