using System;

namespace Invoice.Events
{
    public class InvoiceEmitted
    {
        public InvoiceEmitted(Guid invoiceId, bool success, Guid customerId, string city, string country, string name, string street, string zip)
        {
            InvoiceId = invoiceId;
            Success = success;
            CustomerId = customerId;
            City = city;
            Country = country;
            Name = name;
            Street = street;
            Zip = zip;
        }
        public Guid InvoiceId{ get; }
        public bool Success{ get; }
        public Guid CustomerId{ get; }
        public string City{ get; }
        public string Country{ get; }
        public string Name{ get; }
        public string Street{ get; }
        public string Zip{ get; }
    }
}