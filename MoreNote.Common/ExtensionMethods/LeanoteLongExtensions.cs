using System;
using System.Globalization;

namespace MoreNote.Common.ExtensionMethods
{
    public static class LeanoteLongExtensions
    {
        public static string ToHex(this long number)
        {
            return number.ToString("x");
        }

        public static string ToHex24(this long number)
        {
            return "00000000"+number.ToString("x");
        }
        public static long ToLongByHex(this string hex)
        {   
            //if (hex.Length == 24)
            //{
            //    hex = hex.Substring(0, 16);
            //}
            //119993f42d821000
            long result;
            long.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
            return result;
        }
    }
}