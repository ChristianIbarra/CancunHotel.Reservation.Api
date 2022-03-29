using System;

namespace CancunHotel.Reservation.Domain.Utilities
{
    public class Clock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
