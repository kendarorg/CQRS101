﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs05.Test.Domain.Payment.Commands
{
    public class PaymentCompleted
    {
        public PaymentCompleted(Guid id)
        {
            PaymentId = id;
        }
        public Guid PaymentId { get; set; }
    }
}
