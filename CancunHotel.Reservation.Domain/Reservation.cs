using System;

namespace CancunHotel.Reservation.Domain
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string UserId { get; set; }
    }
}
