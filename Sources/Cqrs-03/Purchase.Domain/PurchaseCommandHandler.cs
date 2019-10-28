#define USE_SCHEDULER
using System;
using System.Linq;
using Bus;
using Crud;
using Newtonsoft.Json;
using PayPal.Shared;
using Purchase.Shared.Commands;
using Purchase.Shared.Events;
using Scheduler;
using Warehouse.Shared.Commands;
using Warehouse.Shared.Events;

// ReSharper disable RedundantLambdaSignatureParentheses
namespace Purchase.Domain
{
    public class PurchaseCommandHandler
    {
        private readonly IBus _bus;
        private readonly IOptimisticRepository<PurchaseEntity> _repository;
        private readonly IPayWithPayPal _payWithPayPal;
#if USE_SCHEDULER
        private readonly IScheduler _scheduler;
#endif

#if USE_SCHEDULER
        public PurchaseCommandHandler(IBus bus, IOptimisticRepository<PurchaseEntity> repository,
            IPayWithPayPal payWithPayPal, IScheduler scheduler)
#else
        public PurchaseCommandHandler(IBus bus, IOptimisticRepository<PurchaseEntity> repository,
            IPayWithPayPal payWithPayPal)
#endif
        {
            _bus = bus;
            _bus.RegisterQueue<PurchaseItems>(Handle);
            _bus.RegisterTopic<ItemsReserved>(Handle);
            _bus.RegisterTopic<PayPalPaymentConcluded>(Handle);
            _bus.RegisterQueue<ExpirePurchase>(Handle);
            _repository = repository;
            _payWithPayPal = payWithPayPal;
#if USE_SCHEDULER
            _scheduler = scheduler;
            // ReSharper disable once ConvertClosureToMethodGroup
            _scheduler.Register(TimeSpan.FromMinutes(10), (date) => OnExpire(date));
#endif
        }

        

        public void Handle(PurchaseItems command)
        {
            var commandForWarehouse = new ReserveItems(command.ItemType,
                command.Quantity, command.Expiration,
                command.PurchaseId, command.PayPalUser); 
            var purchase = new PurchaseEntity
            {
                Id = command.PurchaseId,
                Expiration = command.Expiration,
                Price = command.Price,
                Quantity = command.Quantity,
                PayPalUser = command.PayPalUser,
                State = PurchaseState.Reserving
            };
            _repository.Save(purchase);
#if !USE_SCHEDULER
            _bus.Send(new ExpirePurchase(purchase.Id), purchase.Expiration);
#endif
            _bus.Send(commandForWarehouse);
        }



        public void Handle(ItemsReserved evt)
        {
            var purchase = _repository.GetById(evt.PurchaseId);
            if (evt.Success && purchase.State==PurchaseState.Reserving)
            {
                purchase.PayPalId = _payWithPayPal.PayAndNotify(purchase.PayPalUser, purchase.Price);
                purchase.State = PurchaseState.Paying;
                _repository.Save(purchase);
            }
            else
            {
                SendFailedPurchase(purchase, null);
            }
        }

        public void Handle(PayPalPaymentConcluded evt)
        {
            var purchase = _repository.GetAll(
                    p => p.PayPalId == evt.PayPalPaymentId)
                .First();
            if (evt.Success && purchase.State==PurchaseState.Paying)
            {
                purchase.Expiration = DateTime.MaxValue;
                purchase.State = PurchaseState.Completed;
                _repository.Save(purchase);
                _bus.Send(new ItemsPurchased(purchase.Id,
                    purchase.Price, purchase.Quantity,
                    evt.PayPalPaymentId, true));
            }
            else
            {
                SendFailedPurchase(purchase, evt.PayPalPaymentId);
            }
        }

        private void SendFailedPurchase(PurchaseEntity purchase,
            Guid? payPalPaymentId)
        {
            purchase.State = PurchaseState.Failed;
            _repository.Save(purchase);
            _bus.Send(new ItemsPurchased(purchase.Id,
                purchase.Price, purchase.Quantity,
                payPalPaymentId ?? Guid.Empty, false));
        }

        

        public void Handle(ExpirePurchase command)
        {
            var purchase = _repository.GetById(command.PurchaseId);
            if (purchase.State != PurchaseState.Paying)
            {
                purchase.Expiration = DateTime.MaxValue;
                SendFailedPurchase(purchase, null);
            }
            else
            {
#if !USE_SCHEDULER
                _bus.Send(command, purchase.Expiration);
#endif
                _payWithPayPal.CancelPayment(purchase.PayPalId);
                
            }
        }
#if USE_SCHEDULER
        public void OnExpire(DateTime dateTime)
        {
            foreach (var expired in _repository.GetAll(
                a => a.Expiration < dateTime))
            {
                _bus.Send(new ExpirePurchase(expired.Id));
            }
        }
#endif
    }
}