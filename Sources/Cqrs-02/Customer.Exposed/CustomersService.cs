using Customer.Services;
using System;
using Crud;

namespace Customer
{
    public class CustomersService : ICustomersService
    {
        private readonly IRepository<CustomerEntity> _customersRepository;

        public CustomersService(IRepository<CustomerEntity> customersRepository)
        {
            _customersRepository = customersRepository;
        }
        
        public CustomerData GetCustomerById(Guid id)
        {
            var result = _customersRepository.GetById(id);
            return new CustomerData(result.Id, result.UserName,result.BillingAddress);
        }
    }
}
