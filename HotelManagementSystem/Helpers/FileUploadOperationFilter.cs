using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HotelManagementSystem.Helpers
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasFile = context.ApiDescription.ParameterDescriptions
            .Any(p => p.Type == typeof(IFormFile));

            if (!hasFile) return;

            operation.RequestBody = new OpenApiRequestBody
            {
                Content =
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties =
                        {
                            ["profileImg"] = new OpenApiSchema
                            {
                                Type = "string",
                                Format = "binary"
                            },
                            ["userName"] = new OpenApiSchema { Type = "string" },
                            ["address"] = new OpenApiSchema { Type = "string" },
                            ["gender"] = new OpenApiSchema { Type = "string" },
                            ["dateOfBirth"] = new OpenApiSchema { Type = "string", Format = "date" }
                        }
                    }
                }
            }
            };
        }
    }
}
