using System;

namespace CancunHotel.Reservation.Application.DTOs
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
