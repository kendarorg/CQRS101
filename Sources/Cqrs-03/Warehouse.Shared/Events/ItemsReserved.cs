using System;

namespace Warehouse.Shared.Events
{
    public class ItemsReserved
    {
        public Guid PurchaseId { get; set; }
        public bool Success { get; set; }
    }
}