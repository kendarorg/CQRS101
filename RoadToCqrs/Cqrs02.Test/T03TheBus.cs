using System;
using System.Collections.Generic;
using Cqrs01.Test.Domain;
using Cqrs01.Test.Domain.Commands;
using Cqrs01.Test.Domain.Events;
using Cqrs01.Test.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cqrs02.Test
{
    [TestClass]
    public class T03TheBus
    {
        [TestMethod]
        public void ShouldSendDataViaBus()
        {
            //Given
            var events = new List<object>();
            Guid id = Guid.NewGuid();
            const string name = "test";
            var bus = new Cqrs02.Test.Infrastructure.Bus();
            bus.AddListener(ob =>
            {
                events.Add(ob);
            });
            var eventStore = new EntityStorage(bus);
            var target = new CruiseCommandHandler(bus, eventStore);

            //When
            bus.Send(new CreateCruise(id, name));
            bus.Send(new AddRoom(id, 1, 2));


            //Then
            Assert.AreEqual(4, events.Count);
            var inventoryItemCreated = events[1] as CruiseCreated;

            Assert.AreEqual(id, inventoryItemCreated.CruiseId);
            Assert.AreEqual(name, inventoryItemCreated.Name);

            var roomAdded = events[3] as RoomAdded;

            Assert.AreEqual(id, roomAdded.CruiseId);
            Assert.AreEqual(1, roomAdded.Number);
            Assert.AreEqual(2, roomAdded.Category);
        }
    }
}
