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
    internal class InMemoryFileIO : IFileIO
    {
        readonly Dictionary<string, byte[]> _db = new();
        public async Task CreateAsync(string path, Stream source, CancellationToken token)
        {
            using var mem = new MemoryStream();
            await source.CopyToAsync(mem, token);
            _db.Add(path, mem.ToArray());
        }

        public void Delete(string path)
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
