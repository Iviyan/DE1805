using DE1805Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DE1805Api
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext() : base() { }
        public ApplicationContext(DbContextOptions options) : base(options) { }

        public DbSet<CarFillingStation> CarFillingStations { get; set; }
        public DbSet<CarFillingStationData> CarFillingStationsData { get; set; }

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
