using System;
using System.ComponentModel.DataAnnotations;

namespace CancunHotel.Reservation.Application.DTOs
{
    public class CreateReservation
    {
        private DateTime fromDate;
        private DateTime toDate;

        [Required]
        public DateTime FromDate
        {
            get => fromDate;
            set => fromDate = value.Date;
        }

        [Required]
        public DateTime ToDate
        {
            get => toDate;
            set => toDate = value.Date;
        }
    }
}
