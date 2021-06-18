using MoreNote.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoreNote.Common.ExtensionMethods
{
    public static class JsonExtensions
    {
        public  static string ToJsonForLeanote<T>(this T t)
        {
            string json = JsonSerializer.Serialize(t, MyJsonConvert.GetOptions());
            return json;
        }
        public static string ToJsonForSimple<T>(this T t)
        {
            string json = JsonSerializer.Serialize(t, MyJsonConvert.GetSimpleOptions());
            return json;
        }
    }
}
