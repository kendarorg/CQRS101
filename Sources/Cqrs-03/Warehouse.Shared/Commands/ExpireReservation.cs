using System;

namespace Warehouse.Shared.Commands
{
    public class ExpireReservation
    {
        public Guid ReservationId { get; set; }
        public Guid ProductId { get; set; }
    }
}
