using System;

namespace Invoice.Commands
{
    public class CreateInvoice
    {
        public Guid InvoiceId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
