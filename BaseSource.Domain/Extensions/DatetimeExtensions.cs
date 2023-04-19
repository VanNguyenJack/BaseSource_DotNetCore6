using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Domain.Extensions
{
    public static class DatetimeExtensions
    {
        public static CultureInfo info = CultureInfo.GetCultureInfo("en-US");

        public static string FormatValueTypeDateTime(this DateTime? dt, string formatSpecfier, CultureInfo culture)
        {
            if (dt.HasValue)
            {
                return dt.Value.ToString(formatSpecfier, culture);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string FormatValueTypeDateTime(this DateTime dt, string formatSpecfier, CultureInfo culture)
        {
            return dt.ToString(formatSpecfier, culture);
        }

        public static string ToStringEx(this DateTime? dt, string formatSpecfier)
        {
            if (dt.HasValue)
            {
                return dt.Value.ToString(formatSpecfier);
            }
            else
            {
                return string.Empty;
            }
        }

        public static DateTime? GetValueEx(this DateTime? dt)
        {
            if (dt == null || !dt.HasValue || dt == DateTime.MinValue)
            {
                return null;
            }
            return dt.Value;
        }

        public static DateTime SetEndTime(this DateTime? dt)
        {
            if (dt == null || !dt.HasValue || dt == DateTime.MinValue)
            {
                return DateTime.MinValue;
            }
            return dt.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        public static DateTime SetEndTime(this DateTime dt)
        {
            return dt.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }
    }
}
