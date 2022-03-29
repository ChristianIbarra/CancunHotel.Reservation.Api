using System;

namespace CancunHotel.Reservation.Domain.Utilities
{
    public interface IClock
    {
        public DateTime UtcNow { get; }
    }
}
