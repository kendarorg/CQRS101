using System;

namespace Invoice.Events
{
    public class InvoiceCreated
    {
        public InvoiceCreated(Guid invoiceId, Guid customerId, bool success)
        {
            InvoiceId = invoiceId;
            CustomerId = customerId;
            Success = success;
        }
        public Guid InvoiceId{ get; }
        public Guid CustomerId{ get; }
        public bool Success{ get; set; }
    }
}