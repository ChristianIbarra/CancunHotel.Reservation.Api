using CancunHotel.Reservation.Domain.Exceptions;
using CancunHotel.Reservation.Domain.Utilities;
using Moq;
using System;
using Xunit;

namespace CancunHotel.Reservation.Test.ServiceTests
{
    public class ValidationServiceTests
    {
        private readonly ValidationService validationService;
        private readonly Mock<IClock> clockMock;

        public ValidationServiceTests()
        {
            clockMock = new Mock<IClock>();

            validationService = new ValidationService(clockMock.Object);
        }

        [Theory]
        [ClassData(typeof(DateRangeData))]
        public void ValidateDateRange_ShouldThrow(DateTime fromDate, DateTime toDate, string expectedError)
        {
            // Arrange
            var utcNow = new DateTime(2022, 6, 1);
            clockMock.Setup(clock => clock.UtcNow).Returns(utcNow);

            // Act
            var ex = Assert.Throws<ValidationException>(() => validationService.ValidateDateRange(fromDate, toDate));

            // Assert
            Assert.NotNull(ex);
            Assert.Equal(expectedError, ex.Message);
        }

        private class DateRangeData : TheoryData<DateTime, DateTime, string>
        {
            public DateRangeData()
            {
                Add(new DateTime(2022, 6, 10), new DateTime(2022, 6, 9), "Invalid date range.");
                Add(new DateTime(2022, 6, 10), new DateTime(2022, 6, 10), "Invalid date range.");

                Add(new DateTime(2022, 6, 1), new DateTime(2022, 6, 3), "Reservations start at least the next day of booking.");
                Add(new DateTime(2022, 5, 1), new DateTime(2022, 5, 3), "Reservations start at least the next day of booking.");

                Add(new DateTime(2022, 7, 2), new DateTime(2022, 7, 5), "The room cannot be reserved more than 30 days in advance.");

                Add(new DateTime(2022, 6, 2), new DateTime(2022, 6, 6), "The stay cannot be longer than 3 days.");
                Add(new DateTime(2022, 6, 10), new DateTime(2022, 6, 14), "The stay cannot be longer than 3 days.");
            }
        }
    }
}
