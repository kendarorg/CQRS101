using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PayPal.Shared;
using Purchase.Domain;
using Purchase.Shared.Commands;
using Purchase.Shared.Events;
using Scheduler;
using TestUtils;
using Warehouse.Shared.Commands;
using Warehouse.Shared.Events;

namespace Cqrs_03.Test
{
    [TestClass]
    public class InvoiceUnitTest: BaseTestOptimisticEntity<PurchaseEntity>
    {
        private PurchaseCommandHandler _target;
        private Mock<IPayWithPayPal> _payWithPayPal;
        private Mock<IScheduler> _scheduler;

        [TestInitialize]
        public void Setup()
        {
            Initialize();
            _messages = new List<object>();
            _payWithPayPal = new Mock<IPayWithPayPal>();
            _scheduler = new Mock<IScheduler>();
            _target = new PurchaseCommandHandler(_bus.Object, _repository.Object,
                _payWithPayPal.Object, _scheduler.Object);
        }

        [TestMethod]
        public void PurchasingShouldRequireItemsReservation()
        {
            //Given
            var purchaseId = Guid.NewGuid();
            const double price = 10.0;
            const int quantity = 10;
            var expiration = DateTime.UtcNow;
            var newPurchase = new PurchaseItems
            {
                PayPalUser = "a@b.com",
                PurchaseId = purchaseId,
                Price = price,
                Quantity = quantity,
                Expiration = expiration
            };

            //When
            _target.Handle(newPurchase);

            //Then
            Assert.AreEqual(1, _messages.Count);
            var message = _messages[0] as ReserveItems;
            Assert.IsNotNull(message);
            Assert.AreEqual(PurchaseState.Reserving, _saved.State);
        }

        [TestMethod]
        public void OnConfirmedReservationShouldAskForPayment()
        {
            //Given
            var purchaseId = Guid.NewGuid();
            const double price = 10.0;
            const int quantity = 10;
            var expiration = DateTime.UtcNow;
            _payWithPayPal.Setup(a => a.PayAndNotify(It.IsAny<string>(), It.IsAny<double>()))
                .Returns(Guid.NewGuid());
            _saved = new PurchaseEntity
            {
                Expiration = expiration,
                Id = purchaseId,
                Price = price,
                Quantity = quantity,
                PayPalUser = "a@b.com",
                State = PurchaseState.Reserving
            };
            var itemsReserved = new ItemsReserved
            {
                PurchaseId = purchaseId,
                Success = true
            };

            //When
            _target.Handle(itemsReserved);
            
            //Then
            Assert.AreEqual(0, _messages.Count);
            Assert.AreEqual(PurchaseState.Paying, _saved.State);
            _payWithPayPal.Verify(svc =>
                svc.PayAndNotify(It.IsAny<string>(), It.IsAny<double>()),
                Times.Once);
        }

        [TestMethod]
        public void OnConfirmedPaymentShouldConcludeProcess()
        {
            //Given
            var payPalPaymentId = Guid.NewGuid();
            var purchaseId = Guid.NewGuid();
            const double price = 10.0;
            const int quantity = 10;
            var expiration = DateTime.UtcNow;
            _saved =new PurchaseEntity
            {
                PayPalId = payPalPaymentId,
                Expiration = expiration,
                Id = purchaseId,
                Price = price,
                Quantity = quantity,
                PayPalUser = "a@b.com",
                State = PurchaseState.Paying
            };
            var paymentConcluded = new PayPalPaymentConcluded
            {
                PayPalPaymentId = payPalPaymentId,
                Success = true
            };

            //When
            _target.Handle(paymentConcluded);

            //Then
            Assert.AreEqual(1, _messages.Count);
            var message = _messages[0] as ItemsPurchased;
            Assert.IsNotNull(message);
            Assert.IsTrue(message.Success);
            Assert.AreEqual(PurchaseState.Completed, _saved.State);
        }

        [TestMethod]
        public void OnFailedReservationShouldCancelPurchase()
        {
            //Given
            var purchaseId = Guid.NewGuid();
            const double price = 10.0;
            const int quantity = 10;
            var expiration = DateTime.UtcNow;
            _payWithPayPal.Setup(a => a.PayAndNotify(It.IsAny<string>(), It.IsAny<double>()))
                .Returns(Guid.NewGuid());
            _saved = new PurchaseEntity
            {
                Expiration = expiration,
                Id = purchaseId,
                Price = price,
                Quantity = quantity,
                PayPalUser = "a@b.com",
                State = PurchaseState.Reserving
            };
            var reservationFailed = new ItemsReserved
            {
                PurchaseId = purchaseId,
                Success = false
            };

            //When
            _target.Handle(reservationFailed);

            //Then
            Assert.AreEqual(1, _messages.Count);
            var message = _messages[0] as ItemsPurchased;
            Assert.IsFalse(message.Success);
            Assert.AreEqual(PurchaseState.Failed, _saved.State);
            _payWithPayPal.Verify(svc =>
                svc.PayAndNotify(It.IsAny<string>(), It.IsAny<double>()),
                Times.Never);
        }


        [TestMethod]
        public void OnFailedPaymentShouldCancelPurchase()
        {
            //Given
            var payPalPaymentId = Guid.NewGuid();
            var purchaseId = Guid.NewGuid();
            const double price = 10.0;
            const int quantity = 10;
            var expiration = DateTime.UtcNow;
            _saved = new PurchaseEntity
            {
                PayPalId = payPalPaymentId,
                Expiration = expiration,
                Id = purchaseId,
                Price = price,
                Quantity = quantity,
                PayPalUser = "a@b.com",
                State = PurchaseState.Paying
            };
            var paymentFailed = new PayPalPaymentConcluded
            {
                PayPalPaymentId = payPalPaymentId,
                Success = false
            };

            //When
            _target.Handle(paymentFailed);

            //Then
            Assert.AreEqual(1, _messages.Count);
            var message = _messages[0] as ItemsPurchased;
            Assert.IsNotNull(message);
            Assert.IsFalse(message.Success);
            Assert.AreEqual(PurchaseState.Failed, _saved.State);
        }

        [TestMethod]
        public void OnExpirationOnReservationShouldCancelPurchase()
        {
            //Given
            _saved = new PurchaseEntity
            {
                Id = Guid.NewGuid(),
                Expiration = DateTime.UtcNow,
                State = PurchaseState.Reserving
            };

            //When
            _target.Handle(new ExpirePurchase(_saved.Id));


            //Then
            Assert.AreEqual(1, _messages.Count);
            var message = _messages[0] as ItemsPurchased;
            Assert.IsFalse(message.Success);
            Assert.AreEqual(PurchaseState.Failed, _saved.State);
            _payWithPayPal.Verify(
                a=>a.CancelPayment(It.IsAny<Guid>()),
                Times.Never);
        }

        [TestMethod]
        public void OnExpirationOnPaymentShouldRequestPaypalCancellation()
        {
            //Given
            _saved = new PurchaseEntity
            {
                Id = Guid.NewGuid(),
                Expiration = DateTime.UtcNow,
                State = PurchaseState.Paying
            };

            //When
            _target.Handle(new ExpirePurchase(_saved.Id));


            //Then
            Assert.AreEqual(0, _messages.Count);
            Assert.AreEqual(PurchaseState.Paying, _saved.State);
            _payWithPayPal.Verify(
                a => a.CancelPayment(It.IsAny<Guid>()),
                Times.Once);
        }

        [TestMethod]
        public void ShouldSendAnExpirationMessageForExpiredPurchases()
        {
            _saved = new PurchaseEntity
            {
                Id =Guid.NewGuid(),
                Expiration = DateTime.UtcNow
            };

            //When
            _target.OnExpire(DateTime.UtcNow.AddMinutes(1));

            //Then
            Assert.AreEqual(1, _messages.Count);
            var message = _messages[0] as ExpirePurchase;
            Assert.AreEqual(_saved.Id, message.PurchaseId);
        }

        [TestMethod]
        public void ShouldNotSendAnExpirationMessageForNonExpiredPurchases()
        {
            _saved = new PurchaseEntity
            {
                Expiration = DateTime.MaxValue
            };

            //When
            _target.OnExpire(DateTime.UtcNow.AddMinutes(1));

            //Then
            Assert.AreEqual(0, _messages.Count);
        }
    }
}