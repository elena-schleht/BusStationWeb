using BusStationWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<Models.Route> Routes { get; set; }
        public DbSet<Models.Trip> Trips { get; set; }
        public DbSet<Models.Ticket> Tickets { get; set; }
        public DbSet<Models.Citie> Cities { get; set; }
        public DbSet<Models.Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Trip)
                .WithMany(t => t.Tickets)
                .HasForeignKey(t => t.TripId);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Route)
                .WithMany(r => r.Trips)
                .HasForeignKey(t => t.RouteId);
        }
    }
}