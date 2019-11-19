using System;

namespace Cqrs04.Test.Domains.Invoices.Commands
{
    public class EmitInvoice
    {
        public EmitInvoice(Guid invoiceId,int expectedVersion)
        {
            InvoiceId = invoiceId;
            ExpectedVersion = expectedVersion;
        }
        public Guid InvoiceId { get; set; }
        public int ExpectedVersion { get; set; }
    }
}
