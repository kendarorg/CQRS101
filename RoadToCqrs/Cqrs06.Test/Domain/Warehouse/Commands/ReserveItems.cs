using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs05.Test.Domain.Warehouse.Commands
{
    public class ReserveItems
    {
        public ReserveItems(Guid productId,int quantity,Guid ownerId,DateTime expiration)
        {
            ProductId = productId;
            Quantity = quantity;
            OwnerId = ownerId;
            Expiration = expiration;
        }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime Expiration { get; set; }
    }
}
