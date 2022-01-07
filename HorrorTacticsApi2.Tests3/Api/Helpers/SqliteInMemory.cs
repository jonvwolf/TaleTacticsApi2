using HorrorTacticsApi2.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorTacticsApi2.Tests3.Api.Helpers
{
    public class SqliteInMemory : IDisposable
    {
        readonly DbConnection _conn;
        public readonly DbContextOptions<HorrorDbContext> Options;

        public SqliteInMemory()
        {
            _conn = new SqliteConnection("Filename=:memory:");
            _conn.Open();

            Options = new DbContextOptionsBuilder<HorrorDbContext>().UseSqlite(_conn).Options;
        }

        public HorrorDbContext CreateDbContext()
        {
            return new HorrorDbContext(Options);
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _conn.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
