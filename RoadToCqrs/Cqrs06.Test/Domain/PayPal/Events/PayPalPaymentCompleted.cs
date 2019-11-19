using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs06.Test.Domain.PayPal.Events
{
    public class PayPalPaymentCompleted
    {
        public PayPalPaymentCompleted(Guid ownerId)
        {
            OwnerId = ownerId;
        }

        public Guid OwnerId { get; private set; }
    }
}
