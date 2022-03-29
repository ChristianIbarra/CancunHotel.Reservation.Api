using AutoFixture;
using AutoFixture.Xunit2;
using CancunHotel.Reservation.Domain.Exceptions;
using CancunHotel.Reservation.Domain.Services;
using CancunHotel.Reservation.Domain.Utilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CancunHotel.Reservation.Test.ServiceTests
{
    public class ReservationServiceTests
    {
        private readonly ReservationService reservationService;
        private readonly Mock<IReservationRepository> reservationRepositoryMock;
        private readonly Mock<IUserService> userServiceMock;
        private readonly Mock<IRoomService> roomServiceMock;
        private readonly Mock<IValidationService> validationServiceMock;
        private readonly Fixture fixture;

        public ReservationServiceTests()
        {
            reservationRepositoryMock = new Mock<IReservationRepository>();
            userServiceMock = new Mock<IUserService>();
            roomServiceMock = new Mock<IRoomService>();
            validationServiceMock = new Mock<IValidationService>();
            fixture = new Fixture();

            reservationService = new ReservationService(reservationRepositoryMock.Object, userServiceMock.Object, roomServiceMock.Object, validationServiceMock.Object);
        }

        [Theory, AutoData]
        public async Task CancelReservation(string userId, int reservationId)
        {
            // Arrange
            userServiceMock.Setup(_ => _.GetLoggedInUserId())
                .Returns(userId);

            // Act
            await reservationService.CancelReservation(reservationId);

            // Assert
            reservationRepositoryMock.Verify(_ => _.DeleteReservation(reservationId, userId), Times.Once);
        }

        [Theory, AutoData]
        public async Task EditReservation(string userId, int reservationId, DateTime fromDate, DateTime toDate)
        {
            // Arrange
            userServiceMock.Setup(_ => _.GetLoggedInUserId())
                .Returns(userId);
            roomServiceMock.Setup(_ => _.CheckAvailabilityByDateRange(fromDate, toDate))
                .ReturnsAsync(true);

            // Act
            await reservationService.EditReservation(reservationId, fromDate, toDate);

            // Assert
            reservationRepositoryMock.Verify(_ => _.EditReservation(reservationId, userId, fromDate, toDate), Times.Once);
            roomServiceMock.Verify(_ => _.CheckAvailabilityByDateRange(fromDate, toDate), Times.Once);
            validationServiceMock.Verify(_ => _.ValidateDateRange(fromDate, toDate), Times.Once);
        }

        [Fact]
        public async Task EditReservation_ShouldThrow_ValidationException_InvalidRange()
        {
            // Arrange
            validationServiceMock.Setup(_ => _.ValidateDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(new ValidationException(null));

            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(async () => await reservationService.EditReservation(default, default, default));

            // Assert
            Assert.NotNull(ex);
            userServiceMock.Verify(_ => _.GetLoggedInUserId(), Times.Never);
            reservationRepositoryMock.Verify(_ => _.EditReservation(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            roomServiceMock.Verify(_ => _.CheckAvailabilityByDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async Task EditReservation_ShouldThrow_ValidationException_RoomNotAvailable()
        {
            // Arrange
            roomServiceMock.Setup(_ => _.CheckAvailabilityByDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(false);

            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(async () => await reservationService.EditReservation(default, default, default));

            // Assert
            Assert.NotNull(ex);
            validationServiceMock.Verify(_ => _.ValidateDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            userServiceMock.Verify(_ => _.GetLoggedInUserId(), Times.Never);
            reservationRepositoryMock.Verify(_ => _.EditReservation(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async Task GetReservation()
        {
            // Arrange
            var reservation = fixture.Create<Domain.Reservation>();
            reservationRepositoryMock.Setup(_ => _.GetReservation(reservation.Id))
                .ReturnsAsync(reservation);

            // Act
            var result = await reservationService.GetReservation(reservation.Id);

            // Assert
            Assert.Same(reservation, result);
        }

        [Fact]
        public async Task EditReservation_ShouldThrow_NotFoundException()
        {
            // Act
            var ex = await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await reservationService.GetReservation(1));

            // Assert
            Assert.NotNull(ex);
            Assert.Equal($"{nameof(Domain.Reservation)} not found for id 1", ex.Message);
        }

        [Theory, AutoData]
        public async Task ListUserReservations(string userId)
        {
            // Arrange
            userServiceMock.Setup(_ => _.GetLoggedInUserId())
                .Returns(userId);

            var reservations = new List<Domain.Reservation>();
            reservationRepositoryMock.Setup(_ => _.ListUserReservations(userId))
                .ReturnsAsync(reservations);

            // Act
            var result = await reservationService.ListUserReservations();

            // Assert
            Assert.Same(reservations, result);
        }

        [Theory, AutoData]
        public async Task PlaceReservation(string userId, DateTime fromDate, DateTime toDate)
        {
            // Arrange
            userServiceMock.Setup(_ => _.GetLoggedInUserId())
                .Returns(userId);
            roomServiceMock.Setup(_ => _.CheckAvailabilityByDateRange(fromDate, toDate))
                .ReturnsAsync(true);

            // Act
            var reservationId = await reservationService.PlaceReservation(fromDate, toDate);

            // Assert
            reservationRepositoryMock.Verify(_ => _.CreateReservation(It.Is<Domain.Reservation>(r => 
                r.UserId == userId && r.FromDate == fromDate && r.ToDate == toDate
            )), Times.Once);
            roomServiceMock.Verify(_ => _.CheckAvailabilityByDateRange(fromDate, toDate), Times.Once);
            validationServiceMock.Verify(_ => _.ValidateDateRange(fromDate, toDate), Times.Once);
        }


        [Fact]
        public async Task PlaceReservation_ShouldThrow_ValidationException_InvalidRange()
        {
            // Arrange
            validationServiceMock.Setup(_ => _.ValidateDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(new ValidationException(null));

            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(async () => await reservationService.PlaceReservation(default, default));

            // Assert
            Assert.NotNull(ex);
            userServiceMock.Verify(_ => _.GetLoggedInUserId(), Times.Never);
            reservationRepositoryMock.Verify(_ => _.CreateReservation(It.IsAny<Domain.Reservation>()), Times.Never);
            roomServiceMock.Verify(_ => _.CheckAvailabilityByDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async Task PlaceReservation_ShouldThrow_ValidationException_RoomNotAvailable()
        {
            // Arrange
            roomServiceMock.Setup(_ => _.CheckAvailabilityByDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(false);

            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(async () => await reservationService.PlaceReservation(default, default));

            // Assert
            Assert.NotNull(ex);
            validationServiceMock.Verify(_ => _.ValidateDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            userServiceMock.Verify(_ => _.GetLoggedInUserId(), Times.Never);
            reservationRepositoryMock.Verify(_ => _.CreateReservation(It.IsAny<Domain.Reservation>()), Times.Never);
        }
    }
}
