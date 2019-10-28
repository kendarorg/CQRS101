using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Shared.Commands
{
    public class ExpireReservation
    {
        public Guid ReservationId { get; set; }
        public Guid ProductId { get; set; }
    }
}
