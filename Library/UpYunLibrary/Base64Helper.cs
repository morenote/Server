using System;
using System.Text;

namespace UpYunLibrary
{
    public class Base64Helper
    {
        public static string Encode(string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        public static string FromBase64String(string base64String)
        {
            byte[] outputb = Convert.FromBase64String(base64String);
            return   Encoding.Default.GetString(outputb);
        }
    }
}