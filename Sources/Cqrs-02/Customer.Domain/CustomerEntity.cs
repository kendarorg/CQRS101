using Crud;
using System;

namespace Customer
{
    public class CustomerEntity : IEntity,ILogicalDeleteEntity
    {
        public string UserName { get; set; }
        public Guid Id { get; set; }
        public Guid BillingAddress { get; set; }
        public bool IsDeleted { get; set; }
    }
}
