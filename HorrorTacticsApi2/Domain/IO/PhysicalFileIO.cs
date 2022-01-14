using HorrorTacticsApi2.Domain.Exceptions;

namespace HorrorTacticsApi2.Domain.IO
{
    public class PhysicalFileIO : IFileIO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", 
            "CA1835:Prefer the 'Memory'-based overloads for 'ReadAsync' and 'WriteAsync'", 
            Justification = "Can't make it work with 'Memory'-based overloads")]
        public async Task CreateAsync(string path, Stream source, int maxBytesToRead, CancellationToken token)
        {
            // TODO: copy to a memory stream first?
            // TODO: UI should block for uploading big files, so if file size is exceeded many times by an user, it is trying to do something bad
            using var stream = File.Create(path);

            int totalBytesRead = 0;
            int bytesRead = 0;

            // Buffer size from: https://github.com/microsoft/referencesource/blob/master/mscorlib/system/io/stream.cs
            // `private const int _DefaultCopyBufferSize = 81920;`
            // buffer has to be short lived
            var buffer = new byte[81920];
            
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, token)) > 0)
            {
                await stream.WriteAsync(buffer, 0, bytesRead, token);

                totalBytesRead += bytesRead;
                if (totalBytesRead > maxBytesToRead)
                    throw new HtBadRequestException($"File size exceeds limit. Limit in bytes: {maxBytesToRead}");
            }

            if (totalBytesRead == 0)
                throw new HtBadRequestException($"File size is 0 in bytes");
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }
    }
}
