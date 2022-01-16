using HorrorTacticsApi2.Domain.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HorrorTacticsApi2.Tests3.Api.Helpers
{
    internal class TestInMemoryFileIO : IFileIO
    {
        readonly object locker = new();

        readonly Dictionary<string, byte[]> _db = new();
        public async Task<long> CreateAsync(string path, Stream source, int maxBytes, IReadOnlyList<byte[]> fileSignatures, CancellationToken token)
        {
            using var mem = new MemoryStream();
            await source.CopyToAsync(mem, token);

            lock (locker)
            {
                _db.Add(path, mem.ToArray());
            }

            return source.Length;
        }

        public void Delete(string path)
        {
            lock (locker)
            {
                if (_db.ContainsKey(path))
                {
                    _db.Remove(path);
                }
                else
                {
                    throw new FileNotFoundException($"Test. File not found: {path}");
                }
            }
        }
    }
}
