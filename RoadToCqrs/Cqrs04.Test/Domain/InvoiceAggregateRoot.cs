using Cqrs01.Test.Infrastructure;
using Cqrs04.Test.Domains.Invoices.Customers;
using Cqrs04.Test.Domains.Invoices.Events;
using System;

namespace Cqrs04.Test.Domains.Invoices
{
    public class InvoiceAggregateRoot : AggregateRoot<InvoiceEntity>
    {
        public InvoiceAggregateRoot(InvoiceEntity entity) : base(entity)
        {
        }

        public InvoiceAggregateRoot(Guid id,Guid customerId)
        {
            Entity = new InvoiceEntity(id, customerId);
            Publish(new InvoiceCreated(id, customerId));
        }

        public void EmitInvoice(AddressDataDto address)
        {
            Entity.BillingAddress = address;
            Publish(new InvoiceEmitted(
                Entity.Id,Entity.CustomerId,
                address.City,address.Country,address.Name,address.Street,address.Zip));
        }
    }
}