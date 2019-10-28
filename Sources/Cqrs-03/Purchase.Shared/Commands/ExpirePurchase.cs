using System;

namespace Purchase.Shared.Commands
{
    public class ExpirePurchase
    {
        public ExpirePurchase(Guid purchaseId)
        {
            PurchaseId = purchaseId;
        }
        public Guid PurchaseId { get; }
    }
}
