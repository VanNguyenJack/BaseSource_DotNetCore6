using System;
using System.Diagnostics.CodeAnalysis;

namespace BaseSource.Application.Swagger
{
    public static class SwaggerSchemaIdStrategy
    {
        public static string GenerateCustomSchemaIds(this Type type, [NotNull] string serviceName)
        {
            return $"{serviceName}.{type.GenerateCustomTypeName()}";
        }

        private static string GenerateCustomTypeName(this Type type)
        {
            string result = type.IsGenericType
                ? $"{type.Name.Substring(0, type.Name.IndexOf("`", StringComparison.Ordinal))}.{type.GetGenericArguments()[0].GenerateCustomTypeName()}"
                : type.Name;

            return result.Replace("+", "_");
        }
    }
}
