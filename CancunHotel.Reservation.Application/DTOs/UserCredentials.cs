using System.ComponentModel.DataAnnotations;

namespace CancunHotel.Reservation.Application.DTOs
{
    public class UserCredentials
    {
        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
