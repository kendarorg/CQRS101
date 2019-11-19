using System;
using System.Linq;
using Cqrs01.Test.Domain;
using Cqrs01.Test.Domain.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cqrs01.Test
{
    [TestClass]
    public class T01AggregateRoot
    {
        [TestMethod]
        public void CreatingAnObjectShouldGenerateEvent()
        {
            //Given
            var id = Guid.NewGuid();
            var name = "test";

            //When
            var target = new CruiseAggregateRoot(id, name);

            //Then
            var events = target.GetUnsentEvents().ToList();
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

            //When
            var target = new CruiseAggregateRoot(id, name);

            //Then
            Assert.AreEqual(id, target.Entity.Id);
            Assert.AreEqual(name, target.Entity.Name);
        }

        [TestMethod]
        public void AddingRoomShouldGenerateEvent()
        {
            //Given
            var id = Guid.NewGuid();
            var name = "test";
            var target = new CruiseAggregateRoot(id, name);
            target.ClearEvents();

            //When
            target.AddRoom(1, 2);

            //Then
            var events = target.GetUnsentEvents().ToList();
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
            var target = new CruiseAggregateRoot(id, name);
            target.ClearEvents();

            //When
            target.AddRoom(1, 2);

            //Then
            var roomAdded = target.Entity.Rooms[0];
            Assert.AreEqual(1, roomAdded.Number);
            Assert.AreEqual(2, roomAdded.Category);
        }
    }
}
