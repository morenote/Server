using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Common.ExtensionMethods
{
   public static class StringExtension
    {
        private static readonly ASCIIEncoding asciiEncoding = new ASCIIEncoding();

        public static string ToAscii(this string dirty)
        {
            byte[] bytes = asciiEncoding.GetBytes(dirty);
            string clean = asciiEncoding.GetString(bytes);
            return clean;
        }
    }
}
