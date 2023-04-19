using BaseSource.Domain.Constants;

namespace BaseSource.Domain.Extensions
{
    public static class BooleanExtensions
    {
        public static int ToIntEx(this bool? value)
        {
            if (value == null)
                return 0;
            if (value.HasValue == true)
                return 1;
            return 0;
        }

        public static string GetToString(this bool? value)
        {
            if (value.HasValue)
            {
                return value.Value ? YesNoShortcut.Yes : YesNoShortcut.No;
            }
            return YesNoShortcut.No;
        }
    }
}
