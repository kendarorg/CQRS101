using System;
using System.Collections.Generic;
using Cqrs01.Test.Domain;
using Cqrs01.Test.Domain.Commands;
using Cqrs01.Test.Infrastructure;
using Cqrs02.Test.Projections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cqrs03.Test
{
    [TestClass]
    public class T05OptimisticLock
    {
        [TestMethod]
        public void ShouldVersionAggregateCorrectly()
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
            var target = new EntityStorage(bus);
            var commandHandler = new CruiseCommandHandler(bus, target);
            var cruisesProjection = new CruisesListProjection(bus);

            //When
            bus.Send(new CreateCruise(id, name));
            bus.Send(new AddRoom(id, 1, 2, 0));
            bus.Send(new AddRoom(id, 2, 1, 1));


            //Then
            var cruiseListEntity = cruisesProjection.GetById(id);
            Assert.AreEqual(1, cruiseListEntity.FirstClassRooms);
            Assert.AreEqual(1, cruiseListEntity.SecondClassRooms);
        }


        [TestMethod]
        public void ShouldThrowOnWrongVersion()
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
            var target = new EntityStorage(bus);
            var commandHandler = new CruiseCommandHandler(bus, target);
            
            commandHandler.Handle(new CreateCruise(id, name));
            commandHandler.Handle(new AddRoom(id, 1, 2, 0));

            //When
            Assert.ThrowsException<Exception>(()=>commandHandler.Handle(new AddRoom(id, 2, 1, 0)));
        }
    }
}

        