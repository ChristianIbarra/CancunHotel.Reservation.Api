using CancunHotel.Reservation.Application;
using CancunHotel.Reservation.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CancunHotel.Reservation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserApplication userApplication;

        public UsersController(IUserApplication userApplication)
        {
            this.userApplication = userApplication;
        }

        [HttpPost("login")]
        public async Task<AuthorizationResponse> Login([FromBody] UserCredentials userCredentials)
        {
            return await userApplication.Login(userCredentials);
        }

        [HttpPost("register")]
        public async Task<AuthorizationResponse> Register([FromBody] UserCredentials userCredentials)
        {
            return await userApplication.Register(userCredentials);
        }
    }
}
