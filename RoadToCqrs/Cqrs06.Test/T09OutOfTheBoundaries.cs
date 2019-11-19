using System;
using System.Linq;
using Cqrs01.Test.Infrastructure;
using Cqrs02.Test.Infrastructure;
using Cqrs05.Test.Domain.Payment;
using Cqrs05.Test.Domain.Payment.Commands;
using Cqrs05.Test.Domain.Payment.Events;
using Cqrs05.Test.Domain.Warehouse.Commands;
using Cqrs05.Test.Domain.Warehouse.Events;
using Cqrs06.Test.Domain.Payment.PayPal;
using Cqrs06.Test.Domain.PayPal.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Cqrs06.Test
{
    [TestClass]
    public class T0T9OutOfTheBoundaries
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
            var payPalPaymentId = Guid.NewGuid();
            var payPalPaymentService = new Mock<IPayPalPaymentService>();
            payPalPaymentService.Setup(pps => pps.Pay(
                It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<double>(), It.IsAny<DateTime>()))
                .Returns(payPalPaymentId);
            var target = new PaymentCommandHandler(bus, entityStorage, payPalPaymentService.Object);

            //When
            bus.Send(new CreatePayment(paymentId, productId, customerId, 10.0, 1));
            bus.Send(new ItemsReserved(paymentId));
            bus.Send(new PayPalPaymentCompleted(paymentId));

            //Then
            var entity = entityStorage.GetById<PaymentEntity>(paymentId);
            Assert.AreEqual(customerId, entity.CustomerId);
            Assert.AreEqual(PaymentState.Completed, entity.State);
        }
    }
}
