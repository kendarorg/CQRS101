using System;

namespace Warehouse.Shared.Commands
{
    public class ReserveItems
    {
        public ReserveItems(Guid itemType, long quantity, DateTime expiration, Guid purchaseId, string payPalUser)
        {
            ItemType = itemType;
            Quantity = quantity;
            Expiration = expiration;
            PurchaseId = purchaseId;
            PayPalUser = payPalUser;
        }
        public Guid ItemType{ get; }
        public long Quantity{ get; }
        public DateTime Expiration{ get; }
        public Guid PurchaseId{ get; }
        public string PayPalUser{ get; }
    }
}