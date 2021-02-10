using System;
using System.Globalization;

namespace MoreNote.Common.ExtensionMethods
{
    public static class LeanoteLongExtensions
    {
        public static string ToHex(this long? number)
        {
            return number.Value.ToString("x");
        }

        public static string ToHex24(this long? number)
        {
            return "00000000"+number.Value.ToString("x");
        }
        public static long? ToLongByHex(this string hex)
        {   
            //if (hex.Length == 24)
            //{
            //    hex = hex.Substring(0, 16);
            //}
            //119993f42d821000
            long result;
            if (long.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result))
            {
                return result;
            }else
            {
                return null;
            }
            
        }
          public static long? ToLongByNumber(this string number)
        {   
            //if (hex.Length == 24)
            //{
            //    hex = hex.Substring(0, 16);
            //}
            //119993f42d821000
            long result;
            if (long.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
            {
                return result;
            }else
            {
                return null;
            }
           
        }
    }
}