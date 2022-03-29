using CancunHotel.Reservation.Application.DTOs;
using CancunHotel.Reservation.Domain.Services;
using System;
using System.Threading.Tasks;

namespace CancunHotel.Reservation.Application
{
    public class RoomApplication : IRoomApplication
    {
        private readonly IRoomService roomService;

        public RoomApplication(IRoomService roomService)
        {
            this.roomService = roomService;
        }

        public async Task<RoomAvailabilityResponse> CheckAvailabilityByDateRange(DateTime fromDate, DateTime toDate)
        {
            var isAvailable = await roomService.CheckAvailabilityByDateRange(fromDate, toDate);

            return new RoomAvailabilityResponse
            {
                IsAvailable = isAvailable
            };
        }
    }
}
