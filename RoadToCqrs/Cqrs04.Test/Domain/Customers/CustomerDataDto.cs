using System;

namespace Cqrs04.Test.Domains.Invoices.Customers
{
    public class CustomerDataDto
    {
        public CustomerDataDto(Guid id, string userName, Guid billingAddressId)
        {
            Id = id;
            UserName = userName;
            BillingAddressId = billingAddressId;
        }
        public Guid Id { get; }
        public string UserName { get; }
        public Guid BillingAddressId { get; }
    }
}