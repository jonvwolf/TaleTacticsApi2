namespace HorrorTacticsApi2.Domain.IO
{
    public class PhysicalFileIO : IFileIO
    {
        public async Task CreateAsync(string path, Stream source, CancellationToken token)
        {
            using var stream = File.Create(path);
            await source.CopyToAsync(stream, token);
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }
    }
}
