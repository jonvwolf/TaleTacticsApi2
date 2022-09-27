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

        public TestInMemoryFileIO()
        {
            var files = Directory.GetFiles(Constants.GetDefaultFilePath());
            foreach (var file in files)
            {
                _db.Add(file, new byte[] { 1, 2, 3 });
            }
        }

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

        public Stream GetFileStream(string fullpath)
        {
            lock (locker)
            {
                if (_db.ContainsKey(fullpath))
                {
                    var stream = new MemoryStream(_db[fullpath].ToArray());
                    return stream;
                }
                else
                {
                    throw new FileNotFoundException($"Test. File not found: {fullpath}");
                }
            }
        }
    }
}
