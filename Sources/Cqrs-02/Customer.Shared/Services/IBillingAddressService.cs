using System;

namespace Customer.Services
{
    public interface IBillingAddressService
    {
        BillingAddressData GetBillingAddressForId(Guid customerId);
    }
}
