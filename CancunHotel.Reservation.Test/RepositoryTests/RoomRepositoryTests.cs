using CancunHotel.Reservation.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CancunHotel.Reservation.Test.RepositoryTests
{
    public class RoomRepositoryTests
    {
        private readonly CancunHotelDbContext dbContext;
        private readonly RoomRepository roomRepository;

        public RoomRepositoryTests()
        {
            var builderOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging();

            dbContext = new CancunHotelDbContext(builderOptions.Options);

            roomRepository = new RoomRepository(dbContext);
        }

        [Fact]
        public async Task IsRoomAvailableOnDateRange_Should_Return_False()
        {
            // Arrange
            var fromDate = new DateTime(2022, 6, 10);
            var toDate = new DateTime(2022, 6, 12);
            dbContext.Reservations.Add(new Domain.Reservation
            {
                FromDate = fromDate,
                ToDate = toDate,
                UserId = Guid.NewGuid().ToString(),
            });
            await dbContext.SaveChangesAsync();

            // Act
            var results = new List<bool>
            {
                await roomRepository.IsRoomAvailableOnDateRange(fromDate, toDate),
                await roomRepository.IsRoomAvailableOnDateRange(fromDate.AddDays(-1), fromDate.AddDays(1)),
                await roomRepository.IsRoomAvailableOnDateRange(fromDate, fromDate),
                await roomRepository.IsRoomAvailableOnDateRange(toDate, toDate),
                await roomRepository.IsRoomAvailableOnDateRange(toDate.AddDays(-1), toDate.AddDays(1)),
                await roomRepository.IsRoomAvailableOnDateRange(fromDate.AddDays(-1), toDate.AddDays(1))
            };

            // Assert
            Assert.True(results.All(_ => !_));
        }

        [Fact]
        public async Task IsRoomAvailableOnDateRange_Should_Return_True()
        {
            // Arrange
            var fromDate = new DateTime(2022, 6, 10);
            var toDate = new DateTime(2022, 6, 12);
            dbContext.Reservations.Add(new Domain.Reservation
            {
                FromDate = fromDate,
                ToDate = toDate,
                UserId = Guid.NewGuid().ToString(),
            });
            await dbContext.SaveChangesAsync();

            // Act
            var results = new List<bool>
            {
                await roomRepository.IsRoomAvailableOnDateRange(fromDate.AddDays(-3), fromDate.AddDays(-1)),
                await roomRepository.IsRoomAvailableOnDateRange(toDate.AddDays(1), toDate.AddDays(4))
            };

            // Assert
            Assert.True(results.All(_ => _));
        }
    }
}
