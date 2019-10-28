using System;

namespace PayPal.Shared
{
    public class PayPalPaymentConcluded
    {
        public Guid PayPalPaymentId { get; set; }
        public bool Success { get; set; }
    }
}