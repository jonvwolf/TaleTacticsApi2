namespace HorrorTacticsApi2.Domain.IO
{
    public interface IFileIO
    {
        Task<long> CreateAsync(string path, Stream source, int maxBytesToRead,  IReadOnlyList<byte[]> fileSignatures, CancellationToken token);
        void Delete(string path);
        Stream GetFileStream(string fullpath);
    }
}
