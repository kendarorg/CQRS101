using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs05.Test.Domain.Payment
{
    public enum PaymentState
    {
        None,
        Reserving,
        Failed,
        Confirmed
    }
}
