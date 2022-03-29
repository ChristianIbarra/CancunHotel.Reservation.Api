using CancunHotel.Reservation.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CancunHotel.Reservation.Repository
{
    public class CancunHotelDbContext : IdentityDbContext
    {
        public DbSet<Domain.Reservation> Reservations { get; set; }

        public CancunHotelDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
