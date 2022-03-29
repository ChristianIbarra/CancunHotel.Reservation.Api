using CancunHotel.Reservation.Domain.Exceptions;
using CancunHotel.Reservation.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CancunHotel.Reservation.Domain.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository repository;
        private readonly IUserService userService;
        private readonly IRoomService roomService;
        private readonly IValidationService validationService;

        public ReservationService(
            IReservationRepository repository,
            IUserService userService,
            IRoomService roomService,
            IValidationService validationService
            )
        {
            this.repository = repository;
            this.userService = userService;
            this.roomService = roomService;
            this.validationService = validationService;
        }

        public async Task CancelReservation(int reservationId)
        {
            var userId = userService.GetLoggedInUserId();
            await repository.DeleteReservation(reservationId, userId);
        }

        public async Task EditReservation(int reservationId, DateTime fromDate, DateTime toDate)
        {
            validationService.ValidateDateRange(fromDate, toDate);
            await ValidateRoomAvailability(fromDate, toDate);
            var userId = userService.GetLoggedInUserId();
            await repository.EditReservation(reservationId, userId, fromDate, toDate);
        }

        public async Task<Reservation> GetReservation(int reservationId)
        {
            var reservation = await repository.GetReservation(reservationId);
            if (reservation == null)
            {
                throw new ResourceNotFoundException(reservationId, nameof(Reservation));
            }

            return reservation;
        }

        public async Task<IList<Reservation>> ListUserReservations()
        {
            var userId = userService.GetLoggedInUserId();
            return await repository.ListUserReservations(userId);
        }

        public async Task<int> PlaceReservation(DateTime fromDate, DateTime toDate)
        {
            validationService.ValidateDateRange(fromDate, toDate);
            await ValidateRoomAvailability(fromDate, toDate);
            return await repository.CreateReservation(GetDomainReservation(fromDate, toDate));
        }

        private async Task ValidateRoomAvailability(DateTime fromDate, DateTime toDate)
        {
            var isRoomAvailable = await roomService.CheckAvailabilityByDateRange(fromDate, toDate);
            if (isRoomAvailable)
            {
                return;
            }

            throw new ValidationException($"Room is not available from {fromDate:MM/dd/yyyy} to {toDate:MM/dd/yyyy}");
        }

        private Reservation GetDomainReservation(DateTime fromDate, DateTime toDate)
        {
            var userId = userService.GetLoggedInUserId();
            return new Reservation
            {
                FromDate = fromDate,
                ToDate = toDate,
                UserId = userId
            };
        }
    }
}
