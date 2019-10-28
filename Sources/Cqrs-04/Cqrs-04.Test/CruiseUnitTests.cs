﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cruise;
using Cruise.Commands;
using Cruise.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NServiceBus;
using TestUtils;

namespace Cqrs_04.Test
{
    [TestClass]
    public class CruiseUnitTests: BaseTestOptimisticEntity<CruiseEntity>
    {

        // ReSharper disable once NotAccessedField.Local
        private CruiseCommandHandler _target;
        private Mock<IMessageHandlerContext> _messageHandlerContext;

        private void CreateCruise(Room room = null)
        {
            var cruise = new CruiseEntity(Guid.NewGuid(), 0, "Test", new List<Room>());
            if (room != null)
            {
                cruise.Rooms.Add(room);
            }

            _saved = cruise;
        }


        [TestInitialize]
        public void Setup()
        {
            Initialize();
            _target = new CruiseCommandHandler(_repository.Object);
            _messageHandlerContext = new Mock<IMessageHandlerContext>();
            _messageHandlerContext.Setup(a => a.Publish(It.IsAny<object>(),It.IsAny<PublishOptions>()))
                .Returns((object ob, PublishOptions po) =>
                {
                    _messages.Add(ob);
                    return Task.CompletedTask;
                });
        }

        [TestMethod]
        public void ShouldCreateCruise()
        {
            //Given
            var createCruiseCommand = new CreateCruise("Test");

            //When
            _target.Handle(createCruiseCommand, _messageHandlerContext.Object);


            //Then
            var message = _messages[0] as CruiseCreated;
            Assert.AreEqual(message.Name, _saved.Name);
        }

        [TestMethod]
        public void ShouldAddRoomsToCruise()
        {
            //Given
            CreateCruise();
            var addCruiseRoom = new CreateRoom(_saved.Id, 1, 2, 3);

            //When
            _target.Handle(addCruiseRoom, _messageHandlerContext.Object);

            //Then
            Assert.AreEqual(1, _messages.Count);
            var message = _messages[0] as RoomCreated;
            Assert.IsTrue(message.Success);
            Assert.AreEqual(1, _saved.Rooms.Count);
        }

        [TestMethod]
        public void ShouldNotAddDuplicateRooms()
        {
            //Given
            var newRoom = new Room(Guid.Empty, 1, 1, 1, Guid.NewGuid());
            CreateCruise(newRoom);
            var addCruiseRoom = new CreateRoom(_saved.Id, newRoom.Number, 2, 2);

            //When
            _target.Handle(addCruiseRoom, _messageHandlerContext.Object);

            //Then
            Assert.AreEqual(1, _messages.Count);
            var message = _messages[0] as RoomCreated;
            Assert.IsFalse(message.Success);
            Assert.AreEqual(1, _saved.Rooms.Count);
            Assert.AreEqual(1, _saved.Rooms[0].Class);
        }
    }
}