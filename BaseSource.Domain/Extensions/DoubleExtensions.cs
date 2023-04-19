using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Domain.Extensions
{
    public static class DoubleExtensions
    {
        public static CultureInfo info = CultureInfo.GetCultureInfo("en-US");

        public static double DoubleParse(string str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                str = str.Trim();
            }

            double de = 0;
            if (Double.TryParse(str, NumberStyles.Any, info, out de))
            {
            }

            return de;
        }

        public static double DoubleParse(object o)
        {
            if (o == null)
            {
                return 0;
            }

            double de = 0;
            if (Double.TryParse(o.ToString(), NumberStyles.Any, info, out de))
            {
            }

            return de;
        }

        public static float FloatParse(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Trim();
            }

            float fl = float.Parse(str, CultureInfo.InvariantCulture);
            return fl;
        }

        public static string FormatValueTypeN2(this double number)
        {
            return string.Format(info, "{0:N2}", number);
        }

        public static string FormatValueTypeN2(this double? number)
        {
            if (number.HasValue)
            {
                return number.Value.FormatValueTypeN2();
            }
            else
            {
                return string.Format(info, "{0:N2}", 0);
            }
        }

        public static string FormatValueTypeN3(this double number)
        {
            return string.Format(info, "{0:N3}", number);
        }

        public static string FormatValueTypeN3(this double? number)
        {
            if (number.HasValue)
            {
                return number.Value.FormatValueTypeN3();
            }
            else
            {
                return string.Format(info, "{0:N3}", 0);
            }
        }

        public static decimal ToDecimal(this double number)
        {
            return (decimal)number;
        }
    }
}
