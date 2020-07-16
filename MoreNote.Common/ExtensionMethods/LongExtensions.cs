using System.Globalization;

namespace MoreNote.Common.ExtensionMethods
{
    public static class LongExtensions
    {
        public static string ToHex(this long number)
        {
            return number.ToString("x");
        }

        public static string ToHex24(this long number)
        {
            //11ae1ade40021000aaaaaaaa
            return number.ToString("x")+ "aaaaaaaa";
        }

        public static long ToLong(this string hex)
        {
            //119993f42d821000
            long result = 0;
            if (hex.Length == 16)
            {
            }
            else if (hex.Length == 24)
            {
                hex = hex.Substring(0, 16);
            }
            else
            {
                return 0;
            }
            long.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
            return result;
        }
    }
}