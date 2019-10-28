using Crud;
using System;

namespace Invoice
{
    public class InvoiceEntity : IOptimisticEntity
    {
        public long Version { get; set; }
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
    }
}
