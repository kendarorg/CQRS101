using System;
using Crud;

namespace Customer
{
    public class AddressEntity:IEntity,ILogicalDeleteEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public bool IsDeleted { get; set; }
    }
}