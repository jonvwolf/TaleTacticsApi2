using HorrorTacticsApi2.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HorrorTacticsApi2.Data
{
    public interface IHorrorDbContext
    {
        public DbSet<ImageEntity> Images { get; }
        Task<int> SaveChangesWrappedAsync(CancellationToken cancellationToken = default);
    }
}
