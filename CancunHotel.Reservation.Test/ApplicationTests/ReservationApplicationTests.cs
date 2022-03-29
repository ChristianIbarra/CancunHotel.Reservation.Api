using AutoFixture;
using AutoFixture.Xunit2;
using AutoMapper;
using CancunHotel.Reservation.Application;
using CancunHotel.Reservation.Application.DTOs;
using CancunHotel.Reservation.Domain.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CancunHotel.Reservation.Test.ApplicationTests
{
    public class ReservationApplicationTests
    {
        private readonly ReservationApplication reservationApplication;
        private readonly Mock<IReservationService> reservationServiceMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Fixture fixture;

        public ReservationApplicationTests()
        {
            reservationServiceMock = new Mock<IReservationService>();
            mapperMock = new Mock<IMapper>();
            fixture = new Fixture();

            reservationApplication = new ReservationApplication(reservationServiceMock.Object, mapperMock.Object);
        }

        [Theory, AutoData]
        public async Task CancelReservation(int reservationId)
        {
            // Act
            await reservationApplication.CancelReservation(reservationId);

            // Assert
            reservationServiceMock.Verify(_ => _.CancelReservation(reservationId), Times.Once);
        }

        [Theory, AutoData]
        public async Task EditReservation(int reservationId)
        {
            // Arrange
            var createReservation = fixture.Create<CreateReservation>();

            // Act
            await reservationApplication.EditReservation(reservationId, createReservation);

            // Assert
            reservationServiceMock.Verify(_ => _.EditReservation(reservationId, createReservation.FromDate, createReservation.ToDate), Times.Once);
        }

        [Fact]
        public async Task PlaceReservation()
        {
            // Arrange
            var createReservation = fixture.Create<CreateReservation>();

            // Act
            await reservationApplication.PlaceReservation(createReservation);

            // Assert
            reservationServiceMock.Verify(_ => _.PlaceReservation(createReservation.FromDate, createReservation.ToDate), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetReservation(int reservationId)
        {
            // Arrange
            var domainReservation = new Domain.Reservation();
            var dtoReservation = new Application.DTOs.Reservation();

            mapperMock.Setup(_ => _.Map<Application.DTOs.Reservation>(domainReservation))
                .Returns(dtoReservation);

            reservationServiceMock.Setup(_ => _.GetReservation(reservationId))
                .ReturnsAsync(domainReservation);

            // Act
            var result = await reservationApplication.GetReservation(reservationId);

            // Assert
            Assert.Same(dtoReservation, result);
        }

        [Fact]
        public async Task ListUserReservations()
        {
            // Arrange
            var domainReservations = new List<Domain.Reservation>();
            var dtoReservations = new List<Application.DTOs.Reservation>();

            mapperMock.Setup(_ => _.Map<IList<Application.DTOs.Reservation>>(domainReservations))
                .Returns(dtoReservations);

            reservationServiceMock.Setup(_ => _.ListUserReservations())
                .ReturnsAsync(domainReservations);

            // Act
            var result = await reservationApplication.ListUserReservations();

            // Assert
            Assert.Same(dtoReservations, result);
        }
    }
}
