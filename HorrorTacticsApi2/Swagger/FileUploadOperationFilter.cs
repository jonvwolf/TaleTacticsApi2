using HorrorTacticsApi2.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HorrorTacticsApi2.Swagger
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        static readonly IReadOnlyList<string> OperationIds = new List<string>()
        {
            $"{Constants.ApiPath}/{nameof(ImagesController).Replace("Controller", "")}".ToLowerInvariant(),
            $"{Constants.ApiPath}/{nameof(AudiosController).Replace("Controller", "")}".ToLowerInvariant()
        };

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if ("post".Equals(context.ApiDescription.HttpMethod?.ToLowerInvariant()))
            {
                // TODO: it doesn't work... the UI presents the file but it won't let you click "Execute"
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
                                    Description = "First Document",
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
