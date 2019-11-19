using Cqrs01.Test.Infrastructure;
using Cqrs02.Test.Infrastructure;
using Cqrs05.Test.Domain.Payment.Commands;
using Cqrs05.Test.Domain.Warehouse.Events;
using Cqrs06.Test.Domain.Payment.Commands;
using Cqrs06.Test.Domain.Payment.PayPal;
using Cqrs06.Test.Domain.PayPal.Events;
using System;

namespace Cqrs05.Test.Domain.Payment
{
    public class PaymentCommandHandler
    {
        private readonly IPayPalPaymentService _payPal;
        private readonly Bus _bus;
        private readonly EntityStorage _entityStorage;

        public PaymentCommandHandler(Bus bus, EntityStorage entityStorage, IPayPalPaymentService payPal)
        {
            _payPal = payPal;
            _bus = bus;
            _bus.RegisterQueue<CreatePayment>(Handle);
            _bus.RegisterQueue<ExpirePayment>(Handle);
            _bus.RegisterTopic<ItemsReserved>(Handle);
            _bus.RegisterQueue<CancelPayPalPayment>(Handle);
            _bus.RegisterQueue<PayPalPaymentCompleted>(Handle);
            _entityStorage = entityStorage;
        }

        private void Handle(CreatePayment command)
        {
            var expiration = DateTime.Now + TimeSpan.FromSeconds(1);
            var aggregate = new PaymentAggregateRoot(command.Id, command.ProductId,
                command.UserId, command.Amount, command.Quantity, expiration);
            _entityStorage.Save(command.Id, aggregate);
        }

        private void Handle(ExpirePayment command)
        {
            var entity = _entityStorage.GetById<PaymentEntity>(command.PaymentId);
            var aggregate = new PaymentAggregateRoot(entity);
            aggregate.Expire();
            _entityStorage.Save(command.PaymentId, aggregate, entity.Version);
        }

        private void Handle(ItemsReserved @event)
        {
            var entity = _entityStorage.GetById<PaymentEntity>(@event.OwnerId);
            if (entity != null)
            {
                var aggregate = new PaymentAggregateRoot(entity);
                var payPalTransactionId = _payPal.Pay(
                    entity.Id, entity.CustomerId, 
                    entity.Amount, entity.Expiration);
                aggregate.Pay(payPalTransactionId);
                _entityStorage.Save(@event.OwnerId, aggregate, 0);
            }
        }

        private void Handle(PayPalPaymentCompleted @event)
        {
            var entity = _entityStorage.GetById<PaymentEntity>(@event.OwnerId);
            if (entity != null)
            {
                var aggregate = new PaymentAggregateRoot(entity);
                aggregate.PaymentCompleted();
                _entityStorage.Save(entity.Id, aggregate, entity.Version);
            }
        }

        private void Handle(CancelPayPalPayment @event)
        {
            var entity = _entityStorage.GetById<PaymentEntity>(@event.PaymentId);
            if (entity != null)
            {
                var payPalCanceled = _payPal.Cancel(entity.PayPalTransactionId);
                
                var aggregate = new PaymentAggregateRoot(entity);
                aggregate.PaymentRefunded(payPalCanceled);
                _entityStorage.Save(entity.Id, aggregate, entity.Version);
            }
        }
    }
}
