using Cqrs01.Test.Infrastructure;
using Cqrs02.Test.Infrastructure;
using Cqrs04.Test.Domains.Invoices;
using Cqrs04.Test.Domains.Invoices.Commands;
using Cqrs04.Test.Domains.Invoices.Customers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Cqrs04.Test
{
    [TestClass]
    public class T06AggregateReferences
    {
        [TestMethod]
        public void ShouldAddReferenceToExternalEntities()
        {
            //Given
            var customerId = Guid.NewGuid();
            var customerBillingAddressId = Guid.NewGuid();
            var invoiceId = Guid.NewGuid();
            var bus = new Bus();
            var customersServiceProxy = new Mock<ICustomersServiceProxy>();
            customersServiceProxy
                .Setup(csp => csp.GetCustomerById(It.Is<Guid>(cid => cid == customerId)))
                .Returns(new CustomerDataDto(customerId, "", customerBillingAddressId));
            var entityStorage = new EntityStorage(bus);
            var target = new InvoiceCommandHandler(bus, entityStorage, customersServiceProxy.Object);

            //When
            target.Handle(new CreateInvoice(invoiceId, customerId));

            //Then
            var entity = entityStorage.GetById<InvoiceEntity>(invoiceId);
            Assert.AreEqual(customerId, entity.CustomerId);
            Assert.IsNull(entity.BillingAddress);
        }
    }
}
