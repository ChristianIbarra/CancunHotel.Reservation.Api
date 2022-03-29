using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CancunHotel.Reservation.Domain.Services
{
    public interface IReservationRepository
    {
        Task<int> CreateReservation(Reservation reservation);
        Task<Reservation> GetReservation(int reservationId);
        Task<IList<Reservation>> ListUserReservations(string userId);
        Task DeleteReservation(int reservationId, string userId);
        Task EditReservation(int reservationId, string userId, DateTime fromDate, DateTime toDate);
    }
}
