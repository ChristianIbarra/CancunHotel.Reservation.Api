using CancunHotel.Reservation.Application.DTOs;
using System.Threading.Tasks;

namespace CancunHotel.Reservation.Application
{
    public interface IUserApplication
    {
        Task<AuthorizationResponse> Login(UserCredentials userCredentials);
        Task<AuthorizationResponse> Register(UserCredentials userCredentials);
    }
}
