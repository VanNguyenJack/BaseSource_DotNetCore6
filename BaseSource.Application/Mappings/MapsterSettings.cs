using BaseSource.Domain.DTOs;
using DocumentFormat.OpenXml.Spreadsheet;
using Mapster;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Application.Mappings
{
    public static class MapsterSettings
    {
        public static void ConfigureMapster(this IServiceCollection services)
        {
            //https://github.com/MSaniee/Use-Mapster-In-DotNet
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            Assembly applicationAssembly = typeof(BaseDto<,>).Assembly;
            typeAdapterConfig.Scan(applicationAssembly);
        }
    }
}
