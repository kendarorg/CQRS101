using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs05.Test.Domain.Payment.Commands
{
    public class CreatePayment
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public double Amount { get; set; }
        public int Quantity { get; set; }

        public CreatePayment(Guid id, Guid product, Guid userId, double amount, int quantity)
        {
            Id = id;
            ProductId = product;
            UserId = userId;
            Amount = amount;
            Quantity = quantity;
        }
    }
}
