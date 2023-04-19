using System;

namespace BaseSource.Application.Swagger
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class SwaggerOperationAttribute : Attribute
    {
        public SwaggerOperationAttribute(string operationId)
        {
            OperationId = operationId;
        }

        public string OperationId { get; }
    }
}
