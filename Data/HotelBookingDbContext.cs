using HotelBooking.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace HotelBooking.Data
{

    public class HotelBookingDbContext : IdentityDbContext<IdentityUser>
    {
        public HotelBookingDbContext(DbContextOptions<HotelBookingDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }
        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
            new IdentityRole() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" }
            );
        }

       
        public DbSet<BookingDetails> BookedRoomDetails { get; set; }
        public DbSet<CardDetails> CardDetails { get; set; }
        public DbSet<Hotels> ListOfHotels { get; set; }
        
        public DbSet<UserRegistration> UserRegistrations { get; set; }
        //public DbSet<CustomerWithOrderDetails> CustomerWithOrderDetails { get; set; }
    }

}

