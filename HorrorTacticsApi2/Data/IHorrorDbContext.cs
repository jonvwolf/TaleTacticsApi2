using HorrorTacticsApi2.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace HorrorTacticsApi2.Data
{
    public interface IHorrorDbContext
    {
        public DbSet<FileEntity> Files { get; }
        public DbSet<ImageEntity> Images { get; }
        public DbSet<AudioEntity> Audios { get; }
        public DbSet<StorySceneEntity> StoryScenes { get; }
        public DbSet<StoryEntity> Stories { get; }
        Task<int> SaveChangesWrappedAsync(CancellationToken cancellationToken = default);

        Task<IDbContextTransaction> CreateTransactionAsync();
    }
}
