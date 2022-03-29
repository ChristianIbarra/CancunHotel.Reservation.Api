using AutoFixture.Xunit2;
using CancunHotel.Reservation.Domain.Exceptions;
using CancunHotel.Reservation.Domain.Services;
using CancunHotel.Reservation.Domain.Utilities;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CancunHotel.Reservation.Test.ServiceTests
{
    public class RoomServiceTests
    {
        private readonly RoomService roomService;
        private readonly Mock<IRoomRepository> roomRepositoryMock;
        private readonly Mock<IValidationService> validationServiceMock;

        public RoomServiceTests()
        {
            roomRepositoryMock = new Mock<IRoomRepository>();
            validationServiceMock = new Mock<IValidationService>();

            roomService = new RoomService(roomRepositoryMock.Object, validationServiceMock.Object);
        }

        [Theory, AutoData]
        public async Task CheckAvailabilityByDateRange(DateTime fromDate, DateTime toDate)
        {
            // Arrange
            roomRepositoryMock.Setup(_ => _.IsRoomAvailableOnDateRange(fromDate, toDate))
                .ReturnsAsync(true);

            // Act
            var isAvailable = await roomService.CheckAvailabilityByDateRange(fromDate, toDate);

            // Assert
            Assert.True(isAvailable);
            roomRepositoryMock.Verify(_ => _.IsRoomAvailableOnDateRange(fromDate, toDate), Times.Once);
            validationServiceMock.Verify(_ => _.ValidateDateRange(fromDate, toDate), Times.Once);
        }

        [Fact]
        public async Task CheckAvailabilityByDateRange_ShouldThrow_ValidationException()
        {
            // Arrange
            validationServiceMock.Setup(_ => _.ValidateDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(new ValidationException(string.Empty));

            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(async () => await roomService.CheckAvailabilityByDateRange(default, default));

            // Assert
            Assert.NotNull(ex);
            roomRepositoryMock.Verify(_ => _.IsRoomAvailableOnDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }
    }
}
