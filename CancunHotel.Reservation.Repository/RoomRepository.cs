using CancunHotel.Reservation.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CancunHotel.Reservation.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly CancunHotelDbContext dbContext;

        public RoomRepository(CancunHotelDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> IsRoomAvailableOnDateRange(DateTime fromDate, DateTime toDate)
        {
            return !await dbContext.Reservations
                .AnyAsync(_ => (_.FromDate >= fromDate && _.FromDate <= toDate)
                    || (_.ToDate >= fromDate && _.ToDate <= toDate)
                    || (_.FromDate <= fromDate && _.ToDate >= toDate));
        }
    }
}
