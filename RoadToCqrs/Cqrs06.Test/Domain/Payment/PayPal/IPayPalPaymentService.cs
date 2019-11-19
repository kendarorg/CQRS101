using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs06.Test.Domain.Payment.PayPal
{
    public interface IPayPalPaymentService
    {
        Guid Pay(Guid ownerId, Guid customerId, double amount, DateTime expiration);
        bool Cancel(Guid payPalTransactionId);
    }
}
