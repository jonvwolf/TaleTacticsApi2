using HorrorTacticsApi2.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HorrorTacticsApi2.Data
{
    public class HorrorDbContext : DbContext, IHorrorDbContext
    {
        public DbSet<Image> Images => Set<Image>();

        public HorrorDbContext(DbContextOptions<HorrorDbContext> options) : base(options)
        {

        }

        public Task<int> SaveChangesWrappedAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified).ToList();

            foreach (var entity in entities)
            {
                if (entity is IValidatableEntity validatableEntity)
                {
                    validatableEntity.Validate();
                }
            }

            return SaveChangesAsync(cancellationToken);
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
