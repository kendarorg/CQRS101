using Cqrs03.Test.Infrastructure;
using Cqrs04.Test.Domains.Invoices.Customers;
using System;

namespace Cqrs04.Test.Domains.Invoices
{
    public class InvoiceEntity : IAggregateEntity
    {
        public InvoiceEntity(Guid id, Guid customerId)
        {
            Id = id;
            CustomerId = customerId;
        }

        public int Version { get; set; }
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public AddressDataDto BillingAddress { get; set; }
    }
}
