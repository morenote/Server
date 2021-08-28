using MoreNote.Common.Utils;
using System;
using System.Globalization;
using System.Text;

namespace MoreNote.Common.ExtensionMethods
{
    public static class TypeConverterExtensions
    {
        public static string ToHex(this long? number)
        {
            return number.Value.ToString("x");
        }

        public static string ByteArrayToHex(this byte[] data)
        {
            return HexUtil.ByteArrayToString(data);
        }
        public static byte[] HexToByteArray(this string hex)
        {
            return HexUtil.StringToByteArray(hex);
        }

        public static string ByteArrayToBase64(this byte[] data)
        {
          return  Convert.ToBase64String(data);
        }
        public static byte[] Base64ToByteArray(this string data)
        {
            return Convert.FromBase64String(data);
        }

        public static byte[] ToByteArrayByUtf8(this string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }
        public static string ToStringByUtf8(this byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        public static string ToHexForLeanote(this long? number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return number.Value.ToString("x");
        }

        public static string ToHex24(this long? number)
        {
            return "00000000" + number.Value.ToString("x");
        }
        public static string ToHex24ForLeanote(this long? number)
        {
            if (number==null)
            {
                return string.Empty;
            }
            return "00000000" + number.Value.ToString("x");
        }
        public static string ToHex24ForLeanote(this long number)
        {
            return "00000000" + number.ToString("x");
        }


        public static long? ToLongByHex(this string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                return null;
            }

            //119993f42d821000
            try
            {
                long result = Convert.ToInt64(hex,16);
                return result;
            }
            catch (Exception ex)
            {
                //todo:处理ex

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
        public static bool? ToBool(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            bool result=false;
            bool myBool= Boolean.TryParse(value,out result);
            if (!result)
            {
                return false;
            }
            else
            {
                return myBool;
            }
        
        }

        public static string[] ToTagsArray(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Array.Empty<string>();
            }
            try
            {
                return value.Split(",");
            }
            catch (Exception)
            {

                return Array.Empty<string>();
            }
        }

    

    }

}