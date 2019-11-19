using System;
using Cqrs01.Test.Domain;
using Cqrs01.Test.Domain.Commands;
using Cqrs01.Test.Domain.Events;
using Cqrs01.Test.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cqrs01.Test
{
    [TestClass]
    public class T02CommandHandler
    {
        private EntityStorage _entityStorage;
        private CruiseCommandHandler _target;

        [TestInitialize]
        public void SetUp()
        {
            _entityStorage = new EntityStorage();
            _target = new CruiseCommandHandler(_entityStorage);
        }
        [TestMethod]
        public void CanStoreNewObject()
        {
            //Given
            var id = Guid.NewGuid();
            var name = "test";
            var command = new CreateCruise
            {
                CruiseId = id,
                Name = name
            };

            //When
            _target.Handle(command);

            //Then
            var events = _entityStorage.Events;
            Assert.AreEqual(1, events.Count);
            var cruiseCreated = events[0] as CruiseCreated;
            Assert.AreEqual(id, cruiseCreated.CruiseId);
            Assert.AreEqual(name, cruiseCreated.Name);
        }
        [TestMethod]
        public void CreatingAnObjectShouldModifyState()
        {
            //Given
            var id = Guid.NewGuid();
            var name = "test";
            var command = new CreateCruise
            {
                CruiseId = id,
                Name = name
            };

            //When
            _target.Handle(command);

            //Then
            var entity = _entityStorage.GetById<CruiseEntity>(id);
            Assert.AreEqual(id, entity.Id);
            Assert.AreEqual(name, entity.Name);
        }

        [TestMethod]
        public void AddingRoomShouldGenerateEvent()
        {
            //Given
            var id = Guid.NewGuid();
            var name = "test";
            var aggregateRoot = new CruiseAggregateRoot(id, name);
            _entityStorage.Save(id, aggregateRoot);
            var command = new AddRoom
            {
                CruiseId = id,
                Number = 1,
                Category = 2
            };

            //When
            _target.Handle(command);

            //Then
            var events = _entityStorage.Events;
            Assert.AreEqual(1, events.Count);
            var roomAdded = events[0] as RoomAdded;
            Assert.AreEqual(id, roomAdded.CruiseId);
            Assert.AreEqual(1, roomAdded.Number);
            Assert.AreEqual(2, roomAdded.Category);
        }

        [TestMethod]
        public void AddingRoomShouldModifyState()
        {
            //Given
            var id = Guid.NewGuid();
            var name = "test";
            var aggregateRoot = new CruiseAggregateRoot(id, name);
            _entityStorage.Save(id, aggregateRoot);
            var command = new AddRoom
            {
                CruiseId = id,
                Number = 1,
                Category = 2
            };

            //When
            _target.Handle(command);

            //Then
            var entity = _entityStorage.GetById<CruiseEntity>(id);
            var roomAdded = entity.Rooms[0];
            Assert.AreEqual(1, roomAdded.Number);
            Assert.AreEqual(2, roomAdded.Category);
        }
    }
}
