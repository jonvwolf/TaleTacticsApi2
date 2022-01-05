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

        public override int SaveChanges()
        {
            // TODO: Call Validate on IValidatable entities
            // TODO: Is this the main method for save changes? there are multiple

            return base.SaveChanges();
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
