using System;

namespace Cqrs04.Test.Domains.Invoices.Events
{
    public class InvoiceEmitted
    {
        public InvoiceEmitted(Guid invoiceId,  Guid customerId, string city, string country, string name, string street, string zip)
        {
            InvoiceId = invoiceId;
            CustomerId = customerId;
            City = city;
            Country = country;
            Name = name;
            Street = street;
            Zip = zip;
        }
        public Guid InvoiceId { get; }
        public Guid CustomerId { get; }
        public string City { get; }
        public string Country { get; }
        public string Name { get; }
        public string Street { get; }
        public string Zip { get; }
    }
}