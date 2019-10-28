using System;
using Customer.Services;

namespace Invoice.Externals
{
    public interface ICustomerServices
    {
        BillingAddressData GetBillingAddressForId(Guid customerId);
        
        CustomerData GetCustomerById(Guid id);
    }
}