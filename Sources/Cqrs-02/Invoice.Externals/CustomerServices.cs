using System;
using Customer.Services;
using Invoice.Externals;

namespace Invoice
{
    public class CustomerServices:ICustomerServices
    {
        private readonly ICustomersService _customersService;
        private readonly IBillingAddressService _billingAddressService;

        public CustomerServices(ICustomersService customersService, IBillingAddressService billingAddressService)
        {
            _customersService = customersService;
            _billingAddressService = billingAddressService;
        }

        public BillingAddressData GetBillingAddressForId(Guid customerId)
        {
            return _billingAddressService.GetBillingAddressForId(customerId);
        }

        public CustomerData GetCustomerById(Guid id)
        {
            return _customersService.GetCustomerById(id);
        }
    }
}