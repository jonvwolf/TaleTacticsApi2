using HorrorTacticsApi2.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HorrorTacticsApi2.Swagger
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        static readonly IReadOnlyList<string> OperationIds = new List<string>()
        {
            $"{Constants.SecuredApiPath}/{nameof(ImagesController).Replace("Controller", "")}".ToLowerInvariant(),
            $"{Constants.SecuredApiPath}/{nameof(AudiosController).Replace("Controller", "")}".ToLowerInvariant()
        };

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if ("post".Equals(context.ApiDescription.HttpMethod?.ToLowerInvariant()))
            {
                if (OperationIds.Contains(context.ApiDescription.RelativePath?.ToLowerInvariant()))
                {
                    var multipartBodyPost = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties =
                            {
                                ["file"] = new OpenApiSchema
                                {
                                    Description = "File",
                                    Type = "string",
                                    Format = "binary"
                                }
                            },
                            Required = { "file" }
                        }
                    };

                    operation.RequestBody = new OpenApiRequestBody
                    {
                        Content = { ["multipart/form-data"] = multipartBodyPost }
                    };
                }
            }

        }
    }
}
