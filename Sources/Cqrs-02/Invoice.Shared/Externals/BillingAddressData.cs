﻿namespace Customer.Services
{
    public class BillingAddressData
    {
        public BillingAddressData(string name, string street, string city, string zip, string country)
        {
            Name = name;
            Street = street;
            City = city;
            Zip = zip;
            Country = country;
        }
        public string Name{ get; }
        public string Street{ get; }
        public string City{ get; }
        public string Zip{ get; }
        public string Country{ get; }
    }
}