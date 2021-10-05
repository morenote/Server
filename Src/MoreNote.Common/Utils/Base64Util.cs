using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Common.Utils
{
   public class Base64Util
    {
        public static string UnBase64String(string value)
        {
            if (value == null || value == "")
            {
                return "";
            }
            byte[] bytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(bytes);
        }
        public static string ToBase64String(string value)
        {
            if (value == null || value == "")
            {
                return "";
            }
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }

        public static string ToBase64String(byte[] value)
        {
            if (value == null )
            {
                return "";
            }
            return Convert.ToBase64String(value);
        }
         public static byte[] UnBase6ToByteArray(string value)
        {
            if (value == null || value == "")
            {
                return null;
            }
            byte[] bytes = Convert.FromBase64String(value);
            return bytes;
        }
    }
}
