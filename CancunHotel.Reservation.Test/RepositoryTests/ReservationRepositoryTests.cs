using AutoFixture;
using AutoFixture.Xunit2;
using CancunHotel.Reservation.Domain.Exceptions;
using CancunHotel.Reservation.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CancunHotel.Reservation.Test.RepositoryTests
{
    public class ReservationRepositoryTests
    {
        private readonly CancunHotelDbContext dbContext;
        private readonly ReservationRepository reservationRepository;
        private readonly Fixture fixture;

        public ReservationRepositoryTests()
        {

            var builderOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging();

            dbContext = new CancunHotelDbContext(builderOptions.Options);

            reservationRepository = new ReservationRepository(dbContext);
            fixture = new Fixture();
        }

        [Fact]
        public async Task CreateReservation()
        {
            // Arrange
            var reservation = fixture.Build<Domain.Reservation>()
                .Without(_ => _.Id)
                .Create();

            // Act
            var id = await reservationRepository.CreateReservation(reservation);

            // Assert
            Assert.Equal(1, reservation.Id);

            var createdReservation = await dbContext.Reservations
                .SingleAsync();

            Assert.Equal(reservation.UserId, createdReservation.UserId);
            Assert.Equal(reservation.FromDate, createdReservation.FromDate);
            Assert.Equal(reservation.ToDate, createdReservation.ToDate);
        }

        [Theory, AutoData]
        public async Task EditReservation(DateTime fromDate, DateTime toDate)
        {
            // Arrange
            var reservation = fixture.Build<Domain.Reservation>()
                .Without(_ => _.Id)
                .Create();

            dbContext.Add(reservation);
            await dbContext.SaveChangesAsync();

            // Act
            await reservationRepository.EditReservation(reservation.Id, reservation.UserId, fromDate, toDate);

            // Assert
            var updatedReservation = await dbContext.Reservations
                .SingleAsync();

            Assert.Equal(fromDate, updatedReservation.FromDate);
            Assert.Equal(toDate, updatedReservation.ToDate);
        }

        [Fact]
        public async Task EditReservation_NotFound_RecordDoesNotExist()
        {
            // Arrange
            var reservation = fixture.Build<Domain.Reservation>()
                .Create();

            // Act
            var ex = await Assert.ThrowsAsync<ResourceNotFoundException>(async () =>
            {
                await reservationRepository.EditReservation(reservation.Id, reservation.UserId, default, default);
            });

            // Assert
            Assert.NotNull(ex);
            Assert.Equal($"{nameof(Domain.Reservation)} not found for id {reservation.Id}", ex.Message);
        }

        [Fact]
        public async Task EditReservation_NotFound_InvalidUserId()
        {
            // Arrange
            var reservation = fixture.Build<Domain.Reservation>()
                .Without(_ => _.Id)
                .Create();

            dbContext.Add(reservation);
            await dbContext.SaveChangesAsync();

            // Act
            var ex = await Assert.ThrowsAsync<ResourceNotFoundException>(async () =>
            {
                await reservationRepository.EditReservation(reservation.Id, Guid.NewGuid().ToString(), default, default);
            });

            // Assert
            Assert.NotNull(ex);
            Assert.Equal($"{nameof(Domain.Reservation)} not found for id {reservation.Id}", ex.Message);
        }

        [Fact]
        public async Task DeleteReservation()
        {
            // Arrange
            var reservation = fixture.Build<Domain.Reservation>()
                .Without(_ => _.Id)
                .Create();

            dbContext.Add(reservation);
            await dbContext.SaveChangesAsync();

            // Act
            await reservationRepository.DeleteReservation(reservation.Id, reservation.UserId);

            // Assert
            var dbReservation = await dbContext.Reservations
                .SingleOrDefaultAsync();

            Assert.Null(dbReservation);
        }

        [Fact]
        public async Task DeleteReservation_NotFound_RecordDoesNotExist()
        {
            // Arrange
            var reservation = fixture.Build<Domain.Reservation>()
                .Create();

            // Act
            var ex = await Assert.ThrowsAsync<ResourceNotFoundException>(async () =>
            {
                await reservationRepository.DeleteReservation(reservation.Id, reservation.UserId);
            });

            // Assert
            Assert.NotNull(ex);
            Assert.Equal($"{nameof(Domain.Reservation)} not found for id {reservation.Id}", ex.Message);
        }

        [Fact]
        public async Task DeleteReservation_NotFound_InvalidUserId()
        {
            // Arrange
            var reservation = fixture.Build<Domain.Reservation>()
                .Without(_ => _.Id)
                .Create();

            dbContext.Add(reservation);
            await dbContext.SaveChangesAsync();

            // Act
            var ex = await Assert.ThrowsAsync<ResourceNotFoundException>(async () =>
            {
                await reservationRepository.DeleteReservation(reservation.Id, Guid.NewGuid().ToString());
            });

            // Assert
            Assert.NotNull(ex);
            Assert.Equal($"{nameof(Domain.Reservation)} not found for id {reservation.Id}", ex.Message);
        }

        [Fact]
        public async Task GetReservation()
        {
            // Arrange
            var reservation = fixture.Build<Domain.Reservation>()
                .Without(_ => _.Id)
                .Create();

            dbContext.Add(reservation);
            await dbContext.SaveChangesAsync();

            // Act
            var dbReservation = await reservationRepository.GetReservation(reservation.Id);

            // Assert
            Assert.Equal(reservation.UserId, dbReservation.UserId);
            Assert.Equal(reservation.FromDate, dbReservation.FromDate);
            Assert.Equal(reservation.ToDate, dbReservation.ToDate);
        }

        [Fact]
        public async Task ListUserReservation()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var reservation1 = fixture.Build<Domain.Reservation>()
                .Without(_ => _.Id)
                .With(_ => _.UserId, userId)
                .Create();

            var reservation2 = fixture.Build<Domain.Reservation>()
                .Without(_ => _.Id)
                .With(_ => _.UserId, userId)
                .Create();

            dbContext.Reservations.AddRange(reservation1, reservation2);
            await dbContext.SaveChangesAsync();

            // Act
            var dbReservations = await reservationRepository.ListUserReservations(userId);

            // Assert
            Assert.Equal(reservation1.FromDate, dbReservations.First().FromDate);
            Assert.Equal(reservation1.ToDate, dbReservations.First().ToDate);

            Assert.Equal(reservation2.FromDate, dbReservations.Last().FromDate);
            Assert.Equal(reservation2.ToDate, dbReservations.Last().ToDate);
        }
    }
}
