using System;

namespace Purchase.Shared.Commands
{
    public class PurchaseItems
    {
        public Guid PurchaseId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime Expiration { get; set; }
        public string PayPalUser { get; set; }
        public Guid ItemType { get; set; }
    }
}