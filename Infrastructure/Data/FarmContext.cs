using API.Helpers;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace API.Data
{
    public class FarmContext : DbContext
    {
        public FarmContext(DbContextOptions<FarmContext> options) : base(options)
        {
        }

        public DbSet<Cow> Cows { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<Farm> Farms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder
           .Entity<Cow>(entity =>
           {
               entity.Property(e => e.State)
                .HasConversion(x => x.ToString(),
                x => (CowState)Enum.Parse(typeof(CowState), x));
           });

            modelBuilder
          .Entity<Sensor>(entity =>
          {
              entity.Property(e => e.State)
               .HasConversion(x => x.ToString(),
               x => (SensorState)Enum.Parse(typeof(SensorState), x));
          });
        }
    }
}
