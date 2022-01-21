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
        public DbConnection Connection { get; }
        
        public SqliteInMemory()
        {
            Connection = new SqliteConnection("Filename=:memory:");
            Connection.Open();
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Connection.Dispose();
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
