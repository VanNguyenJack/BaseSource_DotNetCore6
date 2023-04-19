using Askmethat.Aspnet.JsonLocalizer.JsonOptions;
using Askmethat.Aspnet.JsonLocalizer.Localizer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Application.Localization
{
    public class JsonLocalization
    {
        public string Key { get; set; }
        public Dictionary<string, string> LocalizedValue = new Dictionary<string, string>();
    }
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        readonly IWebHostEnvironment _env;
        readonly IMemoryCache _memCache;
        readonly IOptions<JsonLocalizationOptions> _localizationOptions;

        readonly string _resourcesRelativePath;
        public JsonStringLocalizerFactory(IWebHostEnvironment env, IMemoryCache memCache)
        {
            _env = env;
            _memCache = memCache;
        }

        public JsonStringLocalizerFactory(IWebHostEnvironment env, IMemoryCache memCache, IOptions<JsonLocalizationOptions> localizationOptions)
        {
            _env = env;
            _memCache = memCache;
            _localizationOptions = localizationOptions ?? throw new ArgumentNullException(nameof(localizationOptions));
            _resourcesRelativePath = _localizationOptions.Value.ResourcesPath ?? string.Empty;
        }


        public IStringLocalizer Create(Type resourceSource)
        {
            return new JsonStringLocalizer(_env, _memCache, _resourcesRelativePath, _localizationOptions);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new JsonStringLocalizer(_env, _memCache, _resourcesRelativePath, _localizationOptions);
        }
    }
}