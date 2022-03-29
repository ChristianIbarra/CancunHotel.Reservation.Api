using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CancunHotel.Reservation.Domain.Services
{
    public interface IReservationService
    {
        Task<int> PlaceReservation(DateTime fromDate, DateTime toDate);
        Task<Reservation> GetReservation(int reservationId);
        Task<IList<Reservation>> ListUserReservations();
        Task CancelReservation(int reservationId);
        Task EditReservation(int reservationId, DateTime fromDate, DateTime toDate);
    }
}
