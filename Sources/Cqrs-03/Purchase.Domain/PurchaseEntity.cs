﻿using System;
using Crud;

namespace Purchase.Domain
{
    public class PurchaseEntity : IOptimisticEntity
    {
        public PurchaseState State { get; set; }

        public Guid Id { get; set; }
        public long Version { get; set; }
        
        public int Quantity { get; set; }
        public DateTime Expiration { get; set; }
        public double Price { get; set; }
        public string PayPalUser { get; set; }
        public Guid PayPalId { get; set; }
    }
}