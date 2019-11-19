using Cqrs03.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs05.Test.Domain.Payment
{
    public class PaymentEntity : IAggregateEntity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public double Amount { get; set; }
        public int Quantity { get; set; }
        public DateTime Expiration { get; set; }

        public PaymentEntity(Guid id, 
            Guid product, Guid userId, double amount,int quantity,
            DateTime expiration)
        {
            Id = id;
            ProductId = product;
            CustomerId = userId;
            Amount = amount;
            Quantity = quantity;
            Expiration = expiration;
        }

        public int Version { get; set; }
        public PaymentState State { get; set; }
        public Guid PayPalTransactionId { get;  set; }
    }
}
