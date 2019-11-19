using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs06.Test.Domain.Payment.Events
{
    public class PaypalPaymentInititated
    {
        public PaypalPaymentInititated(Guid id, Guid payPalTransactionId)
        {
            PaymentId = id;
            PayPalPaymentId = payPalTransactionId;
        }

        public Guid PayPalPaymentId { get; set; }
        public Guid PaymentId { get; set; }
    }
}
