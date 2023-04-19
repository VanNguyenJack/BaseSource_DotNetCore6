using BaseSource.Application.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace BaseSource.API.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }

        public static IApplicationBuilder UseRequestLocalizationByCulture(this IApplicationBuilder builder)
        {
            var supportedCultures = LocalizationConstants.SupportedLanguages.Select(l => new CultureInfo(l.Code)).ToArray();
            var localizationOptions = new RequestLocalizationOptions
            {
                SupportedUICultures = supportedCultures,
                SupportedCultures = supportedCultures,
                DefaultRequestCulture = new RequestCulture(supportedCultures.First())
            };
            builder.UseRequestLocalization(localizationOptions);

            var requestProvider = new RouteDataRequestCultureProvider();
            localizationOptions.RequestCultureProviders.Insert(0, requestProvider);
            var locOptions = builder.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            builder.UseRequestLocalization(locOptions.Value);

            builder.UseMiddleware<RequestCultureMiddleware>();
            return builder;
        }
    }
}
