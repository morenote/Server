using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Common.ExtensionMethods
{
    public static class LongExtensions
    {
        public static string Hex(this long number)
        {
            return number.ToString("x");
        }
    }
}
