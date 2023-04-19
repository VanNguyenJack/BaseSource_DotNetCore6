using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BaseSource.Application.Swagger
{
    public class SwaggerOperationNameFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.OperationId = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<SwaggerOperationAttribute>()
                .Select(a => a.OperationId)
                .FirstOrDefault();
        }
    }
}
