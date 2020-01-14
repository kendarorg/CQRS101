using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs06.Test.Domain.PayPal.Commands
{
    public class PayWithPayPal
    {
        public Guid PaymentId { get; set; }
        public Guid CustomerId { get; set; }
        public double Amount { get; set; }
        public DateTime Expiration { get; set; }
    }
}
