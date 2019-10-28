using System;

namespace Customer.Services
{
    public interface ICustomersService
    {
        CustomerData GetCustomerById(Guid id);
    }
}
