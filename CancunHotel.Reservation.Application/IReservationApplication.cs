using CancunHotel.Reservation.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CancunHotel.Reservation.Application
{
    public interface IReservationApplication
    {
        Task<int> PlaceReservation(CreateReservation reservation);
        Task<DTOs.Reservation> GetReservation(int reservationId);
        Task<IList<DTOs.Reservation>> ListUserReservations();
        Task CancelReservation(int reservationId);
        Task EditReservation(int reservationId, CreateReservation reservation);
    }
}
