using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CancunHotel.Reservation.Domain.Services
{
    public interface IRoomRepository
    {
        Task<bool> IsRoomAvailableOnDateRange(DateTime fromDate, DateTime toDate);
    }
}
