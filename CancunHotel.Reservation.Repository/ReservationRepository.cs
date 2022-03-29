using CancunHotel.Reservation.Domain.Exceptions;
using CancunHotel.Reservation.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancunHotel.Reservation.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly CancunHotelDbContext dbContext;

        public ReservationRepository(CancunHotelDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task DeleteReservation(int reservationId, string userId)
        {
            var dbReservation = await dbContext.Reservations
                .SingleOrDefaultAsync(_ => _.Id == reservationId && _.UserId == userId);

            if (dbReservation == null)
            {
                throw new ResourceNotFoundException(reservationId, nameof(Domain.Reservation), null);
            }

            dbContext.Reservations.Remove(dbReservation);
            await dbContext.SaveChangesAsync();
        }

        public async Task<int> CreateReservation(Domain.Reservation reservation)
        {
            dbContext.Add(reservation);
            await dbContext.SaveChangesAsync();

            return reservation.Id;
        }

        public async Task EditReservation(int reservationId, string userId, DateTime fromDate, DateTime toDate)
        {
            var dbReservation = await dbContext.Reservations
                .SingleOrDefaultAsync(_ => _.Id == reservationId && _.UserId == userId);

            if (dbReservation == null)
            {
                throw new ResourceNotFoundException(reservationId, nameof(Domain.Reservation), null);
            }

            dbReservation.FromDate = fromDate;
            dbReservation.ToDate = toDate;

            await dbContext.SaveChangesAsync();
        }

        public async Task<Domain.Reservation> GetReservation(int reservationId)
        {
            return await dbContext.Reservations
                .SingleOrDefaultAsync(_ => _.Id == reservationId);
        }

        public async Task<IList<Domain.Reservation>> ListUserReservations(string userId)
        {
            return await dbContext.Reservations
                .Where(_ => _.UserId == userId)
                .ToListAsync();
        }
    }
}
