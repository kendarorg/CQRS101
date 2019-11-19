using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs06.Test.Domain.Payment.Commands
{
    public class CancelPayPalPayment
    {
        public CancelPayPalPayment(Guid id)
        {
            PaymentId = id; 
        }

        public Guid PaymentId { get; set; }
    }
}
