using BaseSource.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Domain.Extensions
{
    public static class IntExtensions
    {
        public static CultureInfo info = CultureInfo.GetCultureInfo("en-US");

        public static string DecimalToEmptyString(int input)
        {
            if (input == 0)
            {
                return string.Empty;
            }
            else
            {
                return input.ToString("N0", info);
            }
        }

        public static string FormatValueTypeN2(this int number)
        {
            return string.Format(info, "{0:N2}", number);
        }

        public static string FormatValueTypeN2(this int? number)
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

        public static string FormatValueTypeN3(this int number)
        {
            return string.Format(info, "{0:N3}", number);
        }

        public static string FormatValueTypeN3(this int? number)
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

        public static int GetValueEx(this int? number)
        {
            if (number.HasValue)
            {
                return number.Value;
            }
            return 0;
        }

        public static int? IntNParse(string str)
        {
            int i = 0;
            if (Int32.TryParse(str, out i))
            {
                return i;
            }

            return null;
        }

        public static int? IntNParse(object o)
        {
            if (o == null)
            {
                return null;
            }

            int i = 0;
            if (Int32.TryParse(o.ToString(), out i))
            {
                return i;
            }

            return null;
        }

        public static int IntParse(bool b)
        {
            return b ? 1 : 0;
        }

        public static int IntParse(bool? b)
        {
            return (b.HasValue && b.Value) ? 1 : 0;
        }

        public static int IntParse(string str)
        {
            int i = 0;
            Int32.TryParse(str, out i);
            return i;
        }

        public static int ParseInt(this string str)
        {
            return IntParse(str);
        }

        public static int IntParse(float o)
        {
            int i = 0;
            try
            {
                i = Convert.ToInt32(o);
            }
            catch
            {
                i = 0;
            }

            return i;
        }

        public static int IntParse(object o)
        {
            if (o == null)
            {
                return 0;
            }

            int i = 0;
            if (Int32.TryParse(o.ToString(), out i))
            {
            }

            return i;
        }

        public static float IsRoundedOddNumber(this float number)
        {
            return (float)Math.Round(number, NumberDefine.RoundedOdd);

        }

        public static string FormatValueTypeN(this int number)
        {
            return string.Format(info, "{0:N}", number);
        }
    }
}