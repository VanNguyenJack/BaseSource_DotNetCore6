﻿using Askmethat.Aspnet.JsonLocalizer.JsonOptions;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Application.Localization
{
    public class JsonStringLocalizer : JsonStringLocalizerBase, IStringLocalizer
    {
        public JsonStringLocalizer(IWebHostEnvironment env, IMemoryCache memCache, string resourcesRelativePath, IOptions<JsonLocalizationOptions> localizationOptions) : base(env, memCache,
            resourcesRelativePath, localizationOptions)
        {
        }

        public JsonStringLocalizer(IWebHostEnvironment env, IMemoryCache memCache, IOptions<JsonLocalizationOptions> localizationOptions) : base(env, memCache, localizationOptions)
        {
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }
        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetString(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return includeParentCultures
                ? Localization
                    .Select(l =>
                    {
                        var value = GetString(l.Key);
                        return new LocalizedString(l.Key, value ?? l.Key, value == null);
                    })
                : Localization
            .Where(l => l.LocalizedValue.ContainsKey(CultureInfo.CurrentUICulture.Name))
                    .Select(l => new LocalizedString(l.Key, l.LocalizedValue[CultureInfo.CurrentUICulture.Name], false));
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new JsonStringLocalizer(Env, MemCache, ResourcesRelativePath, LocalizationOptions);
        }
        string GetString(string name, CultureInfo cultureInfo = null)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (cultureInfo == null)
                cultureInfo = CultureInfo.CurrentUICulture;

            var item = Localization.FirstOrDefault(x => x.Key == name);
            string value = name;

            if (item == null)
            {
                Localization.Add(new JsonLocalization()
                {
                    Key = name,
                    LocalizedValue = new Dictionary<string, string> { { cultureInfo.Name, name } }
                });
                SaveItems();
            }
            else
            {
                if (item.LocalizedValue.TryGetValue(cultureInfo.Name, out value))
                    return value;

                item.LocalizedValue[cultureInfo.Name] = name;
                SaveItems();
            }

            return value;
        }
    }
}
