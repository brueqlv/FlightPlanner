﻿using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Models
{
    public class FlightDbContext : DbContext
    {
        public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options) { }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Airport> Airports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flight>()
                .HasOne(f => f.From)
                .WithMany()
                .HasForeignKey(f => f.FromId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.To)
                .WithMany()
                .HasForeignKey(f => f.ToId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}