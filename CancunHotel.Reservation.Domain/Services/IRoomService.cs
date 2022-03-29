using System;
using System.Threading.Tasks;

namespace CancunHotel.Reservation.Domain.Services
{
    public interface IRoomService
    {
        Task<bool> CheckAvailabilityByDateRange(DateTime fromDate, DateTime toDate);
    }
}
