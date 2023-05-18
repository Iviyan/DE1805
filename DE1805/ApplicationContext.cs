using DE1805.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DE1805
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext() : base() { }

        public DbSet<CarFillingStation> CarFillingStations { get; set; }
        public DbSet<CarFillingStationData> CarFillingStationsData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=localhost;Database=de1805;Trusted_Connection=True;TrustServerCertificate=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CarFillingStation>()
                .HasMany(s => s.Data)
                .WithOne(s => s.CarFillingStation)
                .HasForeignKey(e => e.CarFillingStationId)
                .IsRequired();
        }
    }
}
