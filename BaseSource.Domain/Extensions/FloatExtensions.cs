using BaseSource.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Domain.Extensions
{
    public static class FloatExtensions
    {
        public static float FormatNumberFloatWithThreeDecimal(this float value)
        {
            string numberString = value.FormatNumberToStringWithThreeDecimal();
            return float.Parse(numberString);
        }

        public static string FormatNumberToStringWithThreeDecimal(this float value)
        {
            return value % 1 != 0 ? Math.Round((float)value, 3).ToString(FormatNumber.ThreeDecimalWithZero) : (value + ".000");
        }

        public static string FormatNumberToStringWithTwoDecimal(this float? value)
        {
            return value % 1 != 0 ? Math.Round((float)value, 2).ToString(FormatNumber.TwoDecimal) : (value + ".00");
        }

        public static string FormatNumberToStringWithTwoDecimal(this float value)
        {
            return value % 1 != 0 ? Math.Round((float)value, 2).ToString(FormatNumber.TwoDecimal) : (value + ".00");
        }
    }
}
