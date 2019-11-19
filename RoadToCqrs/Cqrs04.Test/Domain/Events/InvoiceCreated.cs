using System;

namespace Cqrs04.Test.Domains.Invoices.Events
{
    public class InvoiceCreated
    {

        public InvoiceCreated(Guid id, Guid customerId)
        {
            InvoiceId = id;
            CustomerId = customerId;
        }
        public Guid InvoiceId { get; }
        public Guid CustomerId { get; }
    }
}