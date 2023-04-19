using BaseSource.Domain.Constants;
using System;
using System.Globalization;

namespace BaseSource.Domain.Extensions
{
    public static class DecimalExtensions
    {
        public static CultureInfo info = CultureInfo.GetCultureInfo("en-US");

        public static decimal DecimalParse(string str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                str = str.Trim();
            }

            decimal de = 0M;
            if (Decimal.TryParse(str, NumberStyles.Any, info, out de))
            {
            }

            return de;
        }

        public static decimal DecimalParse(string str, NumberStyles styles)
        {
            if (!String.IsNullOrEmpty(str))
            {
                str = str.Trim();
            }

            try
            {
                decimal de = decimal.Parse(str, NumberStyles.Currency);
                return de;
            }
            catch
            {
                return 0;
            }
        }

        public static decimal DecimalParse(object o)
        {
            if (o == null)
            {
                return 0;
            }

            decimal de = 0M;
            if (Decimal.TryParse(o.ToString(), NumberStyles.Any, info, out de))
            {
            }

            return de;
        }

        public static decimal DecimalParseNotCulture(object o)
        {
            if (o == null)
            {
                return 0;
            }

            decimal de = 0M;
            if (Decimal.TryParse(o.ToString(), out de))
            {
            }

            return de;
        }

        public static string FormatValueTypeN2(this decimal number)
        {
            return string.Format(info, "{0:N2}", number);
        }

        public static string FormatValueTypeN2(this decimal? number)
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

        public static string FormatValueTypeN3(this decimal number)
        {
            return string.Format(info, "{0:N3}", number);
        }

        public static string FormatValueTypeN3(this decimal? number)
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

        public static string FormatValueTypeC(this decimal number)
        {
            string result = string.Format(info, "{0:C}", number);

            if (!String.IsNullOrEmpty(result))
            {
                result = result.Trim();
            }
            return result;
        }

        public static decimal RoundUp<T>(this T input, int decimalPlaces)
        {
            decimal inputInDecimal = DecimalParse(input);
            var multiplier = DecimalParse(Math.Pow(10, Convert.ToDouble(decimalPlaces)));
            return Math.Ceiling(inputInDecimal * multiplier) / multiplier;
        }


        public static double ToDouble(this decimal number)
        {
            return (double)number;
        }

        public static string FormatNumberToStringWithSixZero(this decimal value)
        {
            return value % 1 != 0 ? Math.Round((decimal)value, 6).ToString(FormatNumber.SixZero) : (value + ".00");
        }

    }
}
