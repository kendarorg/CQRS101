using System;

namespace Cqrs04.Test.Domains.Invoices.Commands
{
    public class CreateInvoice
    {
        public CreateInvoice(Guid invoiceId,Guid customerId)
        {
            InvoiceId = invoiceId;
            CustomerId = customerId;
        }
        public Guid InvoiceId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
