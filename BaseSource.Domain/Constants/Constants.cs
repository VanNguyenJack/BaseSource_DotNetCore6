using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Domain.Constants
{
    public class Constants
    {
    }

    public class YesNoShortcut
    {
        public const string No = "N";
        public const string Yes = "Y";
    }

    public static class FormatNumber
    {
        public const string C2 = "C2";
        public const string C28 = "C28";
        public const string C3 = "C3";
        public const string F3 = "F3";
        public const string Format3Decimal = "{0:0.000}";
        public const string FormMatF3 = "{0:F3}";
        public const string N0 = "N0";
        public const string N2 = "N2";
        public const string N3 = "N3";
        public const string N4 = "N4";
        public const string OneDecimal = "{0:0.0}";
        public const string SixZero = "#,##0.000000";
        public const string ThreeDecimal = "#######.000";
        public const string ThreeDecimalWithZero = "######0.000";
        public const string TwoDecimalWithZero = "######0.00";
        public const string TreeZero = "0.000";
        public const string TwoDecimal = "####.00";
        public const string ZeroDecimal = "0.00;0";
    }

    public class NumberDefine
    {
        public const int InvalidPack = 999;
        public const int RoundedOdd = 3;
        public const int Zero = 0;
        public const int One = 1;
        public const int UndefinedCube = 999;
    }

    public static class ConstCharacters
    {
        public const char BackSpace = '\b';
        public const char Caret = '^';
        public const string Colon = ":";
        public const string Comma = ",";
        public const char CommaChar = ',';
        public const string Dots = ".";
        public const char DotsChar = '.';
        public const string Empty = "";
        public const char Field = '|';
        public const char Hyphen = '-';
        public const string Semicolon = ";";
        public const string Space = " ";
        public const char UnderScore = '_';
        public const char Zero = '0';
    }
}
