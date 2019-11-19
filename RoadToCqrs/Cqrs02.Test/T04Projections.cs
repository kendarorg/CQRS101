using System;
using System.Collections.Generic;
using Cqrs01.Test.Domain;
using Cqrs01.Test.Domain.Commands;
using Cqrs01.Test.Infrastructure;
using Cqrs02.Test.Projections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cqrs02.Test
{
    [TestClass]
    public class T04Projections
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
            var commandHandler = new CruiseCommandHandler(bus, eventStore);
            var target = new CruisesListProjection(bus);

            //When
            bus.Send(new CreateCruise(id, name));
            bus.Send(new AddRoom(id, 1, 2));
            bus.Send(new AddRoom(id, 2, 1));


            //Then
            var cruiseListEntity = target.GetById(id);
            Assert.AreEqual(1, cruiseListEntity.FirstClassRooms);
            Assert.AreEqual(1, cruiseListEntity.SecondClassRooms);
        }
    }
}
