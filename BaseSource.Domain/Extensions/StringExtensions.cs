using BaseSource.Domain.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BaseSource.Domain.Extensions
{
    public static class StringExtensions
    {
        public static CultureInfo info = CultureInfo.GetCultureInfo("en-US");

        public static int ComputeModulo10CheckDigit(this string number)
        {
            var sum = 0;
            for (var i = 0; i < number.Length; i++)
            {
                sum += Int32.Parse(number[number.Length - 1 - i].ToString()) * (i % 2 == 0 ? 3 : 1);
            }

            var remainder = sum % 10;
            if (remainder == 0)
                return 0;
            return 10 - remainder;
        }

        public static string ConvertEANToGTIN(this string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new Exception($"{code} is empty");
            if (code.Length != 13)
                throw new Exception($"{code} is not a valid EAN format.");

            string digitCode = code.Substring(0, 2);
            string factureCode = code.Substring(2, 5);
            string productCode = code.Substring(7, 5);
            string remainder = code.Substring(12, 1);

            string temp = remainder + digitCode + factureCode + productCode;

            long odd = 0;
            long even = 0;
            for (int i = 1; i < temp.Length + 1; i++)
            {
                if (i % 2 == 1)
                {
                    odd += Convert.ToInt64(temp.Substring(i - 1, 1));
                }
                else
                {
                    even += Convert.ToInt64(temp.Substring(i - 1, 1));
                }
            }

            odd *= 3;
            long checkDigit = odd + even;
            if (checkDigit % 10 == 0)
                checkDigit = 0;
            else
                checkDigit = (10 - checkDigit % 10);

            string EAN = temp + checkDigit.ToString();
            return EAN;
        }

        public static string FixWidth(this string s, int length)
        {
            if (s.Length < length)
                return s.PadRight(length);
            return s.Substring(0, length);
        }

        public static string FormatDefaultValueTypeN2(this string str)
        {
            if (str.IsEmpty())
            {
                var number = 0;
                return string.Format("{0:N2}", number);
            }

            return string.Format("{0:N2}", DecimalExtensions.DecimalParse(str));
        }

        public static string FormatDefaultValueTypeN3(this string str)
        {
            if (str.IsEmpty())
            {
                var number = 0;
                return string.Format("{0:N3}", number);
            }

            return string.Format("{0:N3}", DecimalExtensions.DecimalParse(str));
        }

        public static string GetCharacteristicOfDecimal(this string value)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(value))
                return result;

            int index = value.IndexOf(".");
            if (index > 0)
            {
                result = value.Substring(0, index);
            }
            else if (index == 0)
            {
                result = "0";
            }
            else
            {
                result = value;
            }
            return result.Trim();
        }

        public static string GetSubString(this string value, int length)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length <= length)
            {
                return value;
            }

            return value.Substring(0, length);
        }

        public static string GetValueAfterDelimeter(this string sValue, string delimeter = " | ", bool getOriginIfNotFound = false)
        {
            var resultValue = String.Empty;
            if (String.IsNullOrEmpty(sValue))
            {
                return resultValue;
            }

            int index = sValue.LastIndexOf(delimeter.Trim());
            if (index >= 0)
            {
                if ((index + 1) < sValue.Length)
                    sValue = sValue.Substring(index + 1);
                else sValue = string.Empty;
            }
            else if (getOriginIfNotFound)
            {
                return sValue.Trim();
            }
            else
            {
                return resultValue;
            }

            return sValue.Trim();
        }

        public static string GetValueBeforeDelimeter(this string sValue, string delimeter = " | ")
        {
            string result = String.Empty;
            if (String.IsNullOrEmpty(sValue))
                return result;
            int index = sValue.IndexOf(delimeter.Trim());
            if (index >= 0)
            {
                if (index > 0)
                {
                    result = sValue.Substring(0, index);
                }
            }
            else
            {
                return sValue.Trim();
            }

            return result.Trim();
        }

        public static string GetValueDelimeter(this string value, bool isBeforceDelimeter = true, string character = "|")
        {
            if (!value.IsEmpty())
            {
                string[] splitValue = value.Split(character);
                if (isBeforceDelimeter)
                {
                    return splitValue[0].TrimEx();
                }
                else
                {
                    if (splitValue.Length > 1)
                    {
                        return splitValue[1].TrimEx();
                    }
                    return string.Empty;
                }
            }

            return value;
        }

        public static bool InBetween(this double value, double start, double end)
        {
            return value >= start && value <= end;
        }

        public static bool IsAllDigits(this string s)
        {
            return Regex.IsMatch(s, "^[0-9]+$");
        }

        public static bool IsEmpty(this string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            return false;
        }

        public static string ToStringEx(this string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            return value.Trim();
        }

        public static bool IsNumber(this string value)
        {
            if (value.IsEmpty())
            {
                return true;
            }
            try
            {
                int isNumber = int.Parse(value);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static string Left(this string param, int length)
        {
            if (param.Length > length)
            {
                string result = param.Substring(0, length);
                return result;
            }
            return param;
        }

        public static string LeftWithPadRight(this string input, int length, string padChar = "")
        {
            if (input == null)
                input = String.Empty;
            if (length > input.Length)
            {
                if (!padChar.IsEmpty())
                    input = input.PadRight(length, padChar[0]);
                return input;
            }

            return input.Substring(0, length);
        }

        public static int LengthEx(this string value)
        {
            if (!value.IsEmpty())
            {
                return value.Length;
            }
            return 0;
        }

        public static string MaxLengthEx(this string value, int maxLength)
        {
            if (!value.IsEmpty())
            {
                value = value.Trim();
                if (value.Length <= maxLength)
                {
                    return value;
                }
                value = value.Substring(0, maxLength);
            }
            return value;
        }

        public static string MaxLengthEx(this string value, int maxLength, string character = "...")
        {
            if (!value.IsEmpty())
            {
                value = value.Trim();
                if (value.Length <= maxLength)
                {
                    return value;
                }
                value = value.Substring(0, maxLength);
                value = value + character;
            }
            return value;
        }

        public static string MergeDelimeter(string value, string text)
        {
            return $"{value} | {text}";
        }

        public static string Mid(this string param, int startIndex, int length)
        {
            if (string.IsNullOrEmpty(param))
            {
                return param;
            }

            if (length >= param.Length)
            {
                return param.Substring(startIndex);
            }
            return param.Substring(startIndex, length);
        }

        public static string PadLeft(string strSource, int intLen)
        {
            if (intLen <= 0)
            {
                return strSource.Trim();
            }

            strSource = strSource.Trim();
            var Len = strSource.Length;
            if (Len > intLen)
                return strSource.Substring(Len - intLen, intLen);
            return strSource.PadLeft(intLen);
        }

        public static string PadLeftWithZero(this string strSource, int intLen)
        {
            strSource = strSource.TrimEx();
            var Len = strSource.Length;

            if (Len > intLen)
                return strSource.Substring(Len - intLen, intLen);
            return strSource.PadLeft(intLen, ConstCharacters.Zero);
        }

        public static string PadRight(string source, short length)
        {
            if (String.IsNullOrEmpty(source))
            {
                return String.Empty;
            }

            if (source.Length > length)
            {
                return source.Substring(0, length);
            }

            return source.PadRight(length);
        }

        public static string PadRightWithZero(this string inputSource, int length)
        {
            if (inputSource.Length > length)
            {
                return inputSource.Substring(0, length - 1);
            }
            else
            {
                return inputSource.PadRight(length, '0');
            }
        }

        public static string ReplaceIgnoringCase(this string s, string oldValue, string newValue)
        {
            return Regex.Replace(s, Regex.Escape(oldValue), (string)EscapeReplacementPattern(newValue),
                RegexOptions.IgnoreCase);
        }

        public static string Right(this string param, int length)
        {
            string result = param.Substring(param.Length - length, length);
            return result;
        }

        public static string Right(this string input, int length, string padChar = "")
        {
            if (input == null)
                input = String.Empty;
            if (length > input.Length)
            {
                if (!padChar.IsEmpty() && !input.IsEmpty())
                    input = input.PadLeft(length, padChar[0]);
                return input;
            }

            return input.Substring(input.Length - length, length);
        }

        public static string RightNull(this string input, int length, string padChar = "")
        {
            if (input == null)
                input = String.Empty;
            if (padChar.IsEmpty())
                padChar = " ";
            if (length > input.Length)
            {
                input = input.PadLeft(length, padChar[0]);
                return input;
            }

            return input.Substring(input.Length - length, length);
        }

        public static string StringNParse(object o)
        {
            return o == null ? String.Empty : o.ToString();
        }

        public static string StringNParse(bool? bo)
        {
            if (bo.HasValue)
            {
                if (bo.Value)
                    return "1";
            }

            return String.Empty;
        }

        public static string StringNParse(this DateTime? dt, string formatDateTime = "")
        {
            if (dt.HasValue)
            {
                try
                {
                    return formatDateTime.IsEmpty() ? dt.Value.ToString() : dt.Value.ToString(formatDateTime);
                }
                catch
                {
                    return String.Empty;
                }
            }

            return String.Empty;
        }

        public static string StringNParse(this DateTime dt, string formatDateTime = "")
        {
            try
            {
                return formatDateTime.IsEmpty() ? dt.ToString() : dt.ToString(formatDateTime);
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string StringNParse(decimal? de)
        {
            if (!de.HasValue || de == 0M)
            {
                return String.Empty;
            }

            return StringParse(de.Value);
        }

        public static string StringParse(string input)
        {
            input = input ?? String.Empty;
            input = input == "null" ? String.Empty : input;
            input = input.Trim();
            string pattern = "\\s+";
            string replacement = " ";
            Regex rgx = new Regex(pattern);
            string result = rgx.Replace(input, replacement);
            return input;
        }

        public static string StringParse(decimal de)
        {
            return String.Format(info, "{0:N}", de);
        }

        public static string StringParse(decimal? de)
        {
            if (!de.HasValue)
            {
                return "0";
            }

            return StringParse(de.Value);
        }

        public static string StringParse(int[] ID)
        {
            if (ID == null)
            {
                return "0";
            }
            else
            {
                string result = String.Empty;
                for (int i = 0; i < ID.Length; i++)
                {
                    result = result + ID[i] + ",";
                }

                if (result.Length > 0)
                    return result.Remove(result.Length - 1);
                return "0";
            }
        }

        public static string StringParse(List<int> ID)
        {
            if (ID == null)
            {
                return "0";
            }
            else
            {
                string result = String.Empty;
                for (int i = 0; i < ID.Count; i++)
                {
                    result = result + ID[i] + ",";
                }

                if (result.Length > 0)
                    return result.Remove(result.Length - 1);
                return "0";
            }
        }

        public static string StringParse(List<string> ID)
        {
            if (ID == null)
            {
                return "";
            }
            else
            {
                string result = String.Empty;
                for (int i = 0; i < ID.Count; i++)
                {
                    result = result + ID[i] + ", ";
                }

                if (result.Length > 0)
                    return result.Remove(result.Length - 1);
                return "";
            }
        }

        public static string StringParse(byte[] b)
        {
            return BitConverter.ToString(b);
        }

        public static string StringParse(object o)
        {
            return o == null ? String.Empty : o.ToString();
        }

        public static string StringParse(bool? bo)
        {
            if (bo.HasValue)
            {
                if (bo.Value)
                    return "1";
            }

            return "0";
        }

        public static string StringParseWithDecimalDegit(decimal de)
        {
            string result = String.Format(info, "{0:C}", de);

            if (!String.IsNullOrEmpty(result))
            {
                result = result.Trim();
            }

            return result;
        }

        public static int StringToNumber(this string value)
        {
            if (value.IsEmpty())
            {
                return 0;
            }
            return int.Parse(value);
        }

        public static string ToUpperEx(this string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return value.Trim().ToUpper();
        }

        public static string TrimEx(this string value)
        {
            if (!value.IsEmpty())
            {
                return value.Trim();
            }

            return value;
        }

        public static string ToLowerEx(this string value)
        {
            if (value.IsEmpty())
            {
                return value;
            }

            return value.Trim().ToLower();
        }

        public static string Truncate(this string s, int length)
        {
            if (s.IsEmpty())
                return string.Empty;

            if (s.Length < length)
                return s;

            return s.Substring(0, length);
        }

        //public static string UpperCase(this string s)
        //{
        //    if (s.Contains(LABEL_SETTING.CaseSensitive, StringComparison.CurrentCultureIgnoreCase))
        //    {
        //        return s.ReplaceIgnoringCase(LABEL_SETTING.CaseSensitive, "");
        //    }
        //    else
        //    {
        //        return s.ToUpperEx();
        //    }
        //}

        public static string XMLRemoveSpecialCharsFromAtrributes(this string str)
        {
            if (!str.IsEmpty())
            {
                str = str.Replace("&", "&amp;");
                str = str.Replace("\"", "&quot;");
                str = str.Replace("\'", "&apos;");
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
            }

            return str;
        }

        private static string EscapeReplacementPattern(string pattern)
        {
            return pattern.IsEmpty() ? "" : pattern.Replace("$", "$$");
        }

        public static string PadLeftZero(this string source, short length)
        {
            if (source.Length > length)
            {
                return source.Substring(source.Length - length, length);
            }

            return source.PadLeft(length, '0');
        }

        public static string MaxLen(this string strSource, int intLen)
        {
            if (string.IsNullOrEmpty(strSource))
            {
                return strSource;
            }
            strSource = strSource.Trim();
            return strSource.Length > intLen ? Left(strSource, intLen) : strSource;
        }
        
        public static string PadlZero(string strSource, int intLen)
        {
            strSource = strSource.Trim();
            var Len = strSource.Length;

            if (Len > intLen)
                return strSource.Substring(Len - intLen, intLen);
            return strSource.PadLeft(intLen, ConstCharacters.Zero);
        }

        public static int ToInt(this string value)
        {
            int result = 0;
            int.TryParse(value, out result);
            return result;
        }
    }
}
