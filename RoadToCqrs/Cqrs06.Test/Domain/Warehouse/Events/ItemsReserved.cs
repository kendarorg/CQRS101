using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs05.Test.Domain.Warehouse.Events
{
    public class ItemsReserved
    {
        public ItemsReserved(Guid ownerId)
        {
            OwnerId = ownerId;
        }
        public Guid OwnerId { get; set; }
    }
}
