using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apex.Venues.Data
{
    public class VenuesDbContext : DbContext
    {
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Suitability> Suitabilities { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        private readonly IHostEnvironment _hostEnv;
        private readonly string DbPath;

        public VenuesDbContext(DbContextOptions<VenuesDbContext> options, IHostEnvironment env) : base(options)
        {
            _hostEnv = env;

            var folder = Environment.SpecialFolder.MyDocuments;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "Apex.Venues.db");

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Suitability>()
                   .HasKey(s => new { s.EventTypeId, s.VenueCode });

            builder.Entity<EventType>()
                   .Property(e => e.Id)
                   .IsFixedLength();

            builder.Entity<Suitability>()
                   .Property(e => e.EventTypeId)
                   .IsFixedLength();

            builder.Entity<Suitability>()
                   .Property(e => e.VenueCode)
                   .IsFixedLength();

            builder.Entity<EventType>()
                   .HasMany(e => e.SuitableVenues)
                   .WithOne(s => s.EventType)
                   .HasForeignKey(s => s.EventTypeId);

            builder.Entity<Venue>()
                   .Property(v => v.Code)
                   .IsFixedLength();

            builder.Entity<Venue>()
                   .HasMany(v => v.SuitableEventTypes)
                   .WithOne(s => s.Venue)
                   .HasForeignKey(s => s.VenueCode);

            builder.Entity<Venue>()
                   .HasMany(v => v.AvailableDates)
                   .WithOne(a => a.Venue)
                   .HasForeignKey(a => a.VenueCode);

            builder.Entity<Availability>()
                   .Property(v => v.VenueCode)
                   .IsFixedLength();

            builder.Entity<Availability>()
                   .HasKey(a => new { a.Date, a.VenueCode });


            builder.Entity<Reservation>()
                   .Property(r => r.Reference)
                   .IsFixedLength();

            builder.Entity<Reservation>()
                   .Property(v => v.VenueCode)
                   .IsFixedLength();

            builder.Entity<Reservation>()
                   .HasOne(r => r.Availability)
                   .WithOne(a => a.Reservation)
                   .IsRequired()
                   // prevent an Availability being deleted if there's a Reservation
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<EventType>()
                   .HasData(
                        new EventType { Id = "CON", Title = "Conference" },
                        new EventType { Id = "MET", Title = "Meeting" },
                        new EventType { Id = "PTY", Title = "Party" },
                        new EventType { Id = "WED", Title = "Wedding" }
                    );
        }
       
    }
}
