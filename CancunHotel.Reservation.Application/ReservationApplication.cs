using AutoMapper;
using CancunHotel.Reservation.Application.DTOs;
using CancunHotel.Reservation.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CancunHotel.Reservation.Application
{
    public class ReservationApplication : IReservationApplication
    {
        private readonly IReservationService reservationService;
        private readonly IMapper mapper;

        public ReservationApplication(IReservationService reservationService, IMapper mapper)
        {
            this.reservationService = reservationService;
            this.mapper = mapper;
        }

        public async Task CancelReservation(int reservationId)
        {
            await reservationService.CancelReservation(reservationId);
        }

        public async Task EditReservation(int reservationId, CreateReservation reservation)
        {
            await reservationService.EditReservation(reservationId, reservation.FromDate, reservation.ToDate);
        }

        public async Task<DTOs.Reservation> GetReservation(int reservationId)
        {
            var reservation = await reservationService.GetReservation(reservationId);

            return mapper.Map<DTOs.Reservation>(reservation);
        }

        public async Task<IList<DTOs.Reservation>> ListUserReservations()
        {
            var reservations = await reservationService.ListUserReservations();

            return mapper.Map<IList<DTOs.Reservation>>(reservations);
        }

        public async Task<int> PlaceReservation(CreateReservation reservation)
        {
            return await reservationService.PlaceReservation(reservation.FromDate, reservation.ToDate);
        }
    }
}
