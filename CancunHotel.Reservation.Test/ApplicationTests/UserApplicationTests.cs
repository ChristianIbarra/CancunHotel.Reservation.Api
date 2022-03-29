using AutoFixture;
using AutoFixture.Xunit2;
using CancunHotel.Reservation.Application;
using CancunHotel.Reservation.Application.DTOs;
using CancunHotel.Reservation.Domain.Services;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CancunHotel.Reservation.Test.ApplicationTests
{
    public class UserApplicationTests
    {
        private readonly UserApplication userApplication;
        private readonly Mock<IUserService> userServiceMock;
        private readonly Fixture fixture;

        public UserApplicationTests()
        {
            userServiceMock = new Mock<IUserService>();
            fixture = new Fixture();

            userApplication = new UserApplication(userServiceMock.Object);
        }

        [Theory, AutoData]
        public async Task Login(string token)
        {
            // Arrange
            var credentials = fixture.Create<UserCredentials>();
            userServiceMock.Setup(_ => _.Login(credentials.Email, credentials.Password))
                .ReturnsAsync(token);

            // Act
            var authResponse = await userApplication.Login(credentials);

            // Assert
            Assert.NotNull(authResponse);
            Assert.Equal(token, authResponse.Token);
        }

        [Theory, AutoData]
        public async Task Register(string token)
        {
            // Arrange
            var credentials = fixture.Create<UserCredentials>();
            userServiceMock.Setup(_ => _.Register(credentials.Email, credentials.Password))
                .ReturnsAsync(token);

            // Act
            var authResponse = await userApplication.Register(credentials);

            // Assert
            Assert.NotNull(authResponse);
            Assert.Equal(token, authResponse.Token);
        }
    }
}
