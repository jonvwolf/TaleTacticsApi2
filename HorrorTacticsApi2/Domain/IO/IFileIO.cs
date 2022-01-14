namespace HorrorTacticsApi2.Domain.IO
{
    public interface IFileIO
    {
        Task CreateAsync(string path, Stream source, CancellationToken token);
        void Delete(string path);
    }
}
