using HCore.Application.Modules.Common.Responses;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HCore.API.Filters
{
    public class HCoreProducesResponseFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var returnType = typeof(BaseResponse<>);

            operation.Responses.TryAdd("200", new OpenApiResponse
            {
                Description = "Success",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(returnType, context.SchemaRepository)
                    }
                }
            });

            operation.Responses.TryAdd("400", new OpenApiResponse { Description = "Bad Request" });
            operation.Responses.TryAdd("404", new OpenApiResponse { Description = "Not Found" });
        }
    }

}
