using System.Threading.Tasks;

namespace CancunHotel.Reservation.Domain.Services
{
    public interface IUserService
    {
        Task<string> Register(string email, string password);
        Task<string> Login(string email, string password);
        string GetLoggedInUserId();
    }
}
