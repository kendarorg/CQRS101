using System;
using System.Collections.Generic;
using System.Linq;
using Bus;
using Crud;
using Customer.Services;
using InMemory.Bus;
using InMemory.Crud;
using Invoice;
using Invoice.Commands;
using Invoice.Events;
using Invoice.Externals;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtils;

// ReSharper disable once NotAccessedField.Local
namespace Cqrs_02.Test
{
    [TestClass]
    public class InvoiceUnitTest: BaseTestOptimisticEntity<InvoiceEntity>
    {
        private Mock<ICustomerServices> _customerServices;
        private InvoiceCommandHandler _target;
     
        private void SetupMockCustomer(Guid customerId, Guid? billingAddressId = null)
        {
            _customerServices.Setup(cs => cs.GetCustomerById(It.Is<Guid>(a => a == customerId)))
                .Returns(new CustomerData(customerId, "test", billingAddressId ?? Guid.Empty));
            if (billingAddressId == null) return;
            _customerServices.Setup(cs => cs.GetBillingAddressForId(It.Is<Guid>(a => a == customerId)))
                .Returns(new BillingAddressData(
                    "name" + billingAddressId.Value,
                    "street" + billingAddressId.Value,
                    "city" + billingAddressId.Value,
                    "zip" + billingAddressId.Value,
                    "country" + billingAddressId.Value));
        }

        private static void VerifyBillingAddressSetOnEvent(Guid customerBillingAddressId, InvoiceEmitted message)
        {
            Assert.AreEqual("city" + customerBillingAddressId, message.City);
            Assert.AreEqual("country" + customerBillingAddressId, message.Country);
            Assert.AreEqual("name" + customerBillingAddressId, message.Name);
            Assert.AreEqual("street" + customerBillingAddressId, message.Street);
            Assert.AreEqual("zip" + customerBillingAddressId, message.Zip);
        }

        private static void VerifyBillingAddressSetOnInvoice(Guid customerBillingAddressId, InvoiceEntity record)
        {
            Assert.AreEqual("city" + customerBillingAddressId, record.City);
            Assert.AreEqual("country" + customerBillingAddressId, record.Country);
            Assert.AreEqual("name" + customerBillingAddressId, record.Name);
            Assert.AreEqual("street" + customerBillingAddressId, record.Street);
            Assert.AreEqual("zip" + customerBillingAddressId, record.Zip);
        }

        [TestInitialize]
        public void Setup()
        {
            Initialize();
            
            _customerServices = new Mock<ICustomerServices>();
            
            _target = new InvoiceCommandHandler(_bus.Object, _repository.Object, 
                _customerServices.Object);
        }

        [TestMethod]
        public void ShouldCreateInvoiceWithExistingCustomer()
        {
            //Given
            var customerId = Guid.NewGuid();
            var invoiceId = Guid.NewGuid();
            SetupMockCustomer(customerId);
            var newInvoice = new CreateInvoice
            {
                InvoiceId = invoiceId,
                CustomerId = customerId
            };

            //When
            _target.Handle(newInvoice);


            //Then
            Assert.AreEqual(1, _messages.Count);
            var message = _messages[0] as InvoiceCreated;
            Assert.IsNotNull(_saved);
            Assert.IsTrue(message.Success);
            Assert.AreEqual(message.CustomerId, _saved.CustomerId);
        }

        [TestMethod]
        public void ShouldNotCreateInvoiceWithNotExistingCustomer()
        {
            //Given
            var customerId = Guid.NewGuid();
            var invoiceId = Guid.NewGuid();

            var newInvoice = new CreateInvoice
            {
                InvoiceId = invoiceId,
                CustomerId = customerId
            };

            //When
            _target.Handle(newInvoice);

            //Then
            Assert.AreEqual(1, _messages.Count);
            var message = _messages[0] as InvoiceCreated;
            Assert.IsFalse(message.Success);
            Assert.IsNull(_saved);
        }


        [TestMethod]
        public void ShouldEmitAnInvoice()
        {
            //Given
            var customerBillingAddressId = Guid.NewGuid();
            _saved = new InvoiceEntity
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid()
            };
            SetupMockCustomer(_saved.CustomerId, customerBillingAddressId);
            var newInvoice = new EmitInvoice
            {
                InvoiceId = _saved.Id
            };

            //When
            _target.Handle(newInvoice);

            //Then
            Assert.AreEqual(1, _messages.Count);
            var message = _messages[0] as InvoiceEmitted;
            Assert.IsNotNull(_saved);
            Assert.IsTrue(message.Success);
            Assert.AreEqual(message.InvoiceId, _saved.Id);
            VerifyBillingAddressSetOnInvoice(customerBillingAddressId, _saved);
            VerifyBillingAddressSetOnEvent(customerBillingAddressId, message);
        }
    }
}