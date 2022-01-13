using HorrorTacticsApi2.Common;
using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Exceptions;
using HorrorTacticsApi2.Domain.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HorrorTacticsApi2.Domain
{
    public class FileUploadHandler
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly AppSettings _options;
        readonly FormOptions _defaultFormOptions = new();
        readonly Regex _filenameRegex = new("[^A-Za-z0-9_ -]", RegexOptions.Compiled);
        readonly ILogger<FileUploadHandler> _logger;

        public FileUploadHandler(IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> settings, ILogger<FileUploadHandler> logger)
        {
            _options = settings.Value;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<FileUploaded> HandleAsync(IReadOnlyDictionary<string, FileFormatEnum> allowedExtensions, CancellationToken token)
        {
            // Documentation:
            // - In case, to get extra parameters: https://makolyte.com/aspdotnet-core-get-posted-form-data-in-an-api-controller/
            // . Or add a "part" in multipart/mixed, you can add form-data and json for example, separated by the boundary
            // Upload doc from microsoft: https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-6.0
            var defaultHttpContext = _httpContextAccessor.HttpContext as DefaultHttpContext;
            int maxSize = defaultHttpContext != default ? defaultHttpContext.FormOptions.MultipartBoundaryLengthLimit : _defaultFormOptions.MultipartBoundaryLengthLimit;

            var request = _httpContextAccessor.HttpContext?.Request ?? throw new InvalidOperationException($"{nameof(_httpContextAccessor.HttpContext)} is null");

            if (!request.HasFormContentType
                || request.ContentType != Constants.MULTIPART_FORMDATA
                || !MediaTypeHeaderValue.TryParse(request.ContentType, out var mediaTypeHeader)
                || string.IsNullOrWhiteSpace(mediaTypeHeader.Boundary.Value)
                || mediaTypeHeader.Boundary.Length > maxSize)
            {
                throw new HtBadRequestException($"Request must be a valid {Constants.MULTIPART_FORMDATA}");
            }
            
            var reader = new MultipartReader(mediaTypeHeader.Boundary.Value, request.Body);
            var section = await reader.ReadNextSectionAsync(token);

            while (section != default)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDispositionHeader);

                if (hasContentDispositionHeader
                    && contentDispositionHeader != default
                    && contentDispositionHeader.DispositionType.Equals(Constants.FORMDATA)
                    && !string.IsNullOrWhiteSpace(contentDispositionHeader.FileName.Value))
                {
                    if (section.Body.Length == 0 || section.Body.Length > _options.GetFileSizeLimitInBytes())
                        throw new HtBadRequestException($"File size is either 0 or exceeds limit. Limit in KB: {_options.FileSizeLimitInKB}");

                    var ext = Path.GetExtension(contentDispositionHeader.FileName.Value);
                    if(!allowedExtensions.TryGetValue(ext.ToLowerInvariant(), out var format))
                        throw new HtBadRequestException("File extension is not valid");

                    string name = Path.GetFileNameWithoutExtension(contentDispositionHeader.FileName.Value);
                    if (name.Length > ValidationConstants.File_Name_MaxStringLength)
                        throw new HtBadRequestException($"File name is too long. Max length: {ValidationConstants.File_Name_MaxStringLength}");

                    string filename = Guid.NewGuid().ToString() + "-" + DateTime.Now.ToString("yyyy_MM_dd", CultureInfo.InvariantCulture) + ext;
                    using var targetStream = File.Create(Path.Combine(_options.UploadPath, filename));
                    // TODO: validate file signature
                    await section.Body.CopyToAsync(targetStream, token);
                    // TODO: scan file ClamAV

                    return new FileUploaded(_filenameRegex.Replace(name, "?"), section.Body.Length, format, filename);
                }

                // To keep reading: section = await reader.ReadNextSectionAsync(token);
            }

            throw new HtBadRequestException($"Request must be a valid {Constants.MULTIPART_FORMDATA}");
        }

        public bool TryDeleteUploadedFile(FileUploaded file)
        {
            try
            {
                File.Delete(Path.Combine(_options.UploadPath, file.Filename));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error trying to delete file: {filename}", file.Filename);
                return false;
            }
        }

        public void DeleteUploadedFile(string filename)
        {
            File.Delete(Path.Combine(_options.UploadPath, filename));
        }
    }
}
