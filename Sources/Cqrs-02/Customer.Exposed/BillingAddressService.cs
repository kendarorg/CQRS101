using Customer.Services;
using System;
using Crud;

namespace Customer
{
    public class BillingAddressService : IBillingAddressService
    {
        private readonly IRepository<CustomerEntity> _customersRepository;
        private readonly IRepository<AddressEntity> _addressesRepository;

        public BillingAddressService(
            IRepository<CustomerEntity> customersRepository,
            IRepository<AddressEntity> addressesRepository)
        {
            _customersRepository = customersRepository;
            _addressesRepository = addressesRepository;
        }

        public BillingAddressData GetBillingAddressForId(Guid customerId)
        {
            var customer = _customersRepository.GetById(customerId);
            var result = _addressesRepository.GetById(customer.BillingAddress);
            return new BillingAddressData(
                result.Name, 
                result.Street, result.City, result.Zip, result.Country);
        }
    }
}