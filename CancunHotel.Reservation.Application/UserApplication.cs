using CancunHotel.Reservation.Application.DTOs;
using CancunHotel.Reservation.Domain.Services;
using System.Threading.Tasks;

namespace CancunHotel.Reservation.Application
{
    public class UserApplication : IUserApplication
    {
        private readonly IUserService userService;

        public UserApplication(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<AuthorizationResponse> Login(UserCredentials userCredentials)
        {
            var token = await userService.Login(userCredentials.Email, userCredentials.Password);

            return new AuthorizationResponse
            {
                Token = token
            };
        }

        public async Task<AuthorizationResponse> Register(UserCredentials userCredentials)
        {
            var token = await userService.Register(userCredentials.Email, userCredentials.Password);

            return new AuthorizationResponse
            {
                Token = token
            };
        }
    }
}
