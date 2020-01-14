using System;
using System.Linq;
using Cqrs01.Test.Infrastructure;
using Cqrs02.Test.Infrastructure;
using Cqrs05.Test.Domain.Payment;
using Cqrs05.Test.Domain.Payment.Commands;
using Cqrs05.Test.Domain.Payment.Events;
using Cqrs05.Test.Domain.Warehouse.Commands;
using Cqrs05.Test.Domain.Warehouse.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cqrs05.Test
{
    [TestClass]
    public class T08DistributedTransaction
    {
        [TestMethod]
        public void ShouldCreatePaymentInStartState()
        {
            //Given
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var paymentId = Guid.NewGuid();
            var bus = new Bus();

            var entityStorage = new EntityStorage(bus);
            var target = new PaymentCommandHandler(bus, entityStorage);

            //When
            bus.Send(new CreatePayment(paymentId, productId, customerId, 10.0, 1));

            //Then
            var entity = entityStorage.GetById<PaymentEntity>(paymentId);
            Assert.AreEqual(customerId, entity.CustomerId);
            Assert.AreEqual(PaymentState.Reserving, entity.State);
        }

        [TestMethod]
        public void ShouldGenerateExpireAndCreateEventsContextually()
        {
            //Given
            var now = DateTime.Now;
            //When
            var target = new PaymentAggregateRoot(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 10.0, 1, now);

            //Then
            var events = target.GetUnsentEvents().ToList();
            Assert.AreEqual(2, events.Count);
            Assert.IsInstanceOfType(events[0].Data, typeof(ExpirePayment));
            Assert.IsInstanceOfType(events[1].Data, typeof(ReserveItems));
        }

        [TestMethod]
        public void ShouldGeneratePaymentCompletedOnItemReserve()
        {
            //Given
            var now = DateTime.Now;
            var entity = new PaymentEntity(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 10.0, 1, now);

            //When
            var target = new PaymentAggregateRoot(entity);
            target.Confirm();

            //Then
            var events = target.GetUnsentEvents().ToList();
            Assert.AreEqual(1, events.Count);
            Assert.IsInstanceOfType(events[0].Data, typeof(PaymentCreated));
        }

        [TestMethod]
        public void ShouldConfirmPaymentWhenDone()
        {
            //Given
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var paymentId = Guid.NewGuid();
            var bus = new Bus();

            var entityStorage = new EntityStorage(bus);
            var target = new PaymentCommandHandler(bus, entityStorage);
            bus.Send(new CreatePayment(paymentId, productId, customerId, 10.0, 1));

            //When
            bus.Send(new ItemsReserved(paymentId));

            //Then
            var entity = entityStorage.GetById<PaymentEntity>(paymentId);
            Assert.AreEqual(customerId, entity.CustomerId);
            Assert.AreEqual(PaymentState.Confirmed, entity.State);
        }


        [TestMethod]
        public void ShouldFailWhenExpiring()
        {
            //Given
            var now = DateTime.Now;
            var entity = new PaymentEntity(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 10.0, 1, now);
            entity.State = PaymentState.Reserving;

            //When
            var target = new PaymentAggregateRoot(entity);
            target.Expire();

            //Then
            var events = target.GetUnsentEvents().ToList();
            Assert.AreEqual(1, events.Count);
            Assert.IsInstanceOfType(events[0].Data, typeof(PaymentFailed));
        }
    }
}
