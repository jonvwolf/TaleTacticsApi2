using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;

namespace HorrorTacticsApi2.Data
{
    public class HorrorDbContext : DbContext, IHorrorDbContext
    {
        public DbSet<FileEntity> Files => Set<FileEntity>();
        public DbSet<ImageEntity> Images => Set<ImageEntity>();
        public DbSet<AudioEntity> Audios => Set<AudioEntity>();
        public DbSet<StorySceneEntity> StoryScenes => Set<StorySceneEntity>();
        public DbSet<StorySceneCommandEntity> StorySceneCommands => Set<StorySceneCommandEntity>();
        public DbSet<StoryEntity> Stories => Set<StoryEntity>();
        public DbSet<UserEntity> Users => Set<UserEntity>();

        public HorrorDbContext(DbContextOptions<HorrorDbContext> options) : base(options)
        {
        }

        public Task<int> SaveChangesWrappedAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified).ToList();

            foreach (var entity in entities)
            {
                if (entity.Entity is IValidatableEntity validatableEntity)
                {
                    validatableEntity.Validate();
                }
            }

            return SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
                .Property(x => x.Role)
                .HasConversion<string>();

            modelBuilder.Entity<FileEntity>()
                .Property(x => x.Format)
                .HasConversion<string>();

            modelBuilder.Entity<StorySceneCommandEntity>()
                .HasMany(x => x.Audios)
                .WithMany(x => x.SceneCommands);

            modelBuilder.Entity<StorySceneCommandEntity>()
                .HasMany(x => x.Images)
                .WithMany(x => x.SceneCommands);

            modelBuilder.Entity<UserEntity>()
                    .HasData(new UserEntity(Constants.AdminUsername, Array.Empty<byte>(), Array.Empty<byte>())
                    {
                        Id = Constants.AdminUserId,
                        Role = UserRole.Admin
                    });
        }

        public Task<IDbContextTransaction> CreateTransactionAsync()
        {
            return Database.BeginTransactionAsync();
        }
    }
}
