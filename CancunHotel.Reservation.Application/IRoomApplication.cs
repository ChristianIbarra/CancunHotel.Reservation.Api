using CancunHotel.Reservation.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace CancunHotel.Reservation.Application
{
    public interface IRoomApplication
    {
        Task<RoomAvailabilityResponse> CheckAvailabilityByDateRange(DateTime fromDate, DateTime toDate);
    }
}
