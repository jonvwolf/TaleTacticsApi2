using HorrorTacticsApi2.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HorrorTacticsApi2.Data
{
    public interface IHorrorDbContext
    {
        public DbSet<FileEntity> Files { get; }
        public DbSet<ImageEntity> Images { get; }
        public DbSet<AudioEntity> Audios { get; }
        Task<int> SaveChangesWrappedAsync(CancellationToken cancellationToken = default);
    }
}
