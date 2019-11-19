using System;

namespace Customer.Services
{
    public class CustomerData{
        public CustomerData(Guid id, string userName, Guid billingAddressId)
        {
            Id = id;
            UserName = userName;
            BillingAddressId = billingAddressId;
        }
        public Guid Id{ get; }
        public string UserName{ get; }
        public Guid BillingAddressId{ get; }
    }
}