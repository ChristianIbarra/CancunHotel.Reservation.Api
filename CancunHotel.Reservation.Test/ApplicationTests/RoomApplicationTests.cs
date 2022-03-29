using CancunHotel.Reservation.Application;
using CancunHotel.Reservation.Domain.Services;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CancunHotel.Reservation.Test.ApplicationTests
{
    public class RoomApplicationTests
    {
        private readonly RoomApplication roomApplication;
        private readonly Mock<IRoomService> roomServiceMock;

        public RoomApplicationTests()
        {
            roomServiceMock = new Mock<IRoomService>();

            roomApplication = new RoomApplication(roomServiceMock.Object);
        }

        [Fact]
        public async Task CheckAvailabilityByDateRange()
        {
            // Arrange
            roomServiceMock.Setup(_ => _.CheckAvailabilityByDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(true);

            // Act
            var result = await roomApplication.CheckAvailabilityByDateRange(default, default);

            // Assert
            Assert.True(result.IsAvailable);
        }
    }
}
