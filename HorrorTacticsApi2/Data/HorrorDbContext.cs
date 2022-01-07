using HorrorTacticsApi2.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HorrorTacticsApi2.Data
{
    public class HorrorDbContext : DbContext
    {
        public DbSet<Image> Images => Set<Image>();

        public HorrorDbContext(DbContextOptions<HorrorDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Image>()
                .Property(x => x.Format)
                .HasConversion<string>();
        }
    }
}
