using HorrorTacticsApi2.Domain.Exceptions;

namespace HorrorTacticsApi2.Domain.IO
{
    public class PhysicalFileIO : IFileIO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", 
            "CA1835:Prefer the 'Memory'-based overloads for 'ReadAsync' and 'WriteAsync'", 
            Justification = "Can't make it work with 'Memory'-based overloads")]
        public async Task<long> CreateAsync(string path, Stream source, int maxBytesToRead, IReadOnlyList<byte[]> fileSignatures, CancellationToken token)
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

            bool signatureValidated = false;
            if (fileSignatures.Count == 0)
                signatureValidated = true;

            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, token)) > 0)
            {
                if (!signatureValidated)
                {
                    bool found = false;
                    foreach (var fileSignature in fileSignatures)
                    {
                        if (ValidateSignature(buffer, fileSignature))
                        {
                            found = true;
                            break;
                        }
                    }
                    // TODO: log alert?
                    if (!found)
                        throw new HtBadRequestException($"Invalid file signature based on file format");
                    signatureValidated = true;
                }

                await stream.WriteAsync(buffer, 0, bytesRead, token);

                totalBytesRead += bytesRead;
                if (totalBytesRead > maxBytesToRead)
                    throw new HtBadRequestException($"File size exceeds limit. Limit in bytes: {maxBytesToRead}");
            }

            if (totalBytesRead == 0)
                throw new HtBadRequestException($"File size is 0 in bytes");

            return totalBytesRead;
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }

        static bool ValidateSignature(byte[] toValidate, byte[] correct)
        {
            if (correct.Length > toValidate.Length)
                throw new InvalidOperationException($"correct length cannot be greater than toValidate length. toValidate: {toValidate.Length}");

            for (int i = 0; i < correct.Length; i++)
            {
                if (correct[i] != toValidate[i])
                    return false;
            }

            return true;
        }
    }
}
