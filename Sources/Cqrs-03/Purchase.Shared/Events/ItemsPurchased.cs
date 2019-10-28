using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Purchase.Shared.Events
{
    public class ItemsPurchased
    {
        public ItemsPurchased(Guid purchaseId, double price, int quantity, Guid paymentId, bool success)
        {
            PurchaseId = purchaseId;
            Price = price;
            Quantity = quantity;
            PaymentId = paymentId;
            Success = success;
        }

        public Guid PurchaseId { get; }
        public double Price { get; }
        public int Quantity { get; }
        public Guid PaymentId { get; }
        public bool Success { get; }
    }
}