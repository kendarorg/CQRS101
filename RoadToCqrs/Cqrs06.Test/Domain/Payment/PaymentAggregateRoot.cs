using Cqrs01.Test.Infrastructure;
using Cqrs05.Test.Domain.Payment.Commands;
using Cqrs05.Test.Domain.Payment.Events;
using Cqrs05.Test.Domain.Warehouse.Commands;
using Cqrs06.Test.Domain.Payment.Commands;
using Cqrs06.Test.Domain.Payment.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs05.Test.Domain.Payment
{
    public class PaymentAggregateRoot : AggregateRoot<PaymentEntity>
    {
        public PaymentAggregateRoot(PaymentEntity entity) : base(entity)
        {
        }

        public PaymentAggregateRoot(Guid id, Guid product, Guid userId,
            double amount, int quantity, DateTime expiration)
        {
            Entity = new PaymentEntity(id, product, userId, amount, quantity, expiration);
            Entity.State = PaymentState.Reserving;
            Publish(new ReserveItems(product, quantity, id, expiration));
            Publish(new ExpirePayment(id), expiration);
        }

        public void Expire()
        {
            if (Entity.State == PaymentState.Reserving)
            {
                Entity.State = PaymentState.Failed;
                Publish(new PaymentFailed(Entity.Id));
            }
            else if (Entity.State == PaymentState.Paying)
            {
                Publish(new CancelPayPalPayment(Entity.Id));
            }
        }

        public void Confirm()
        {
            if (Entity.State == PaymentState.Paying)
            {
                Entity.State = PaymentState.Confirmed;
                Publish(new PaymentCreated(Entity.Id));
            }
        }

        public void Pay(Guid payPalTransactionId)
        {
            if (Entity.State == PaymentState.Reserving)
            {
                Entity.State = PaymentState.Paying;
                Entity.PayPalTransactionId = payPalTransactionId;
                Publish(new PaypalPaymentInititated(Entity.Id, payPalTransactionId));
            }
        }

        public void PaymentRefunded(bool wereAbleToRefund)
        {
            if (Entity.State == PaymentState.Paying)
            {
                if (wereAbleToRefund)
                {
                    Entity.State = PaymentState.Failed;
                    Publish(new PaymentFailed(Entity.Id));
                }
                else
                {
                    Publish(new CancelPayPalPayment(Entity.Id));
                }
            }
        }
    }
}
