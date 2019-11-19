using System;

namespace Cqrs04.Test.Domains.Invoices.Customers
{
    public interface ICustomersServiceProxy
    {
        CustomerDataDto GetCustomerById(Guid customerId);
        AddressDataDto GetBillingAddressForId(Guid customerId);
    }
}
