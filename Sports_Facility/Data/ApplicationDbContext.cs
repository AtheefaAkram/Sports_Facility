using Microsoft.EntityFrameworkCore;
using Sports_Facility.Models;
using System.Reflection.Emit;

namespace Sports_Facility.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberSport> MemberSports { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>().ToTable("Location");
            modelBuilder.Entity<Sport>().ToTable("Sport");
            modelBuilder.Entity<Facility>().ToTable("Facility");
            modelBuilder.Entity<Member>().ToTable("Member");
            modelBuilder.Entity<MemberSport>().ToTable("Member_Sport");
            modelBuilder.Entity<Guest>().ToTable("Guest");
            modelBuilder.Entity<Booking>().ToTable("Booking");
            modelBuilder.Entity<Payment>().ToTable("Payment");
            modelBuilder.Entity<Review>().ToTable("Review");
            modelBuilder.Entity<Inquiry>().ToTable("Inquiry");
        }
    }
}