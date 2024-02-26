using MoreNote.Common.Utils;

using System.Collections.Generic;
using System.Text.Json;

namespace MoreNote.Common.ExtensionMethods
{
	public static class JsonExtensions
	{

		public static string ToJsonForLeanote<T>(this T t)
		{
			string json = JsonSerializer.Serialize(t, MyJsonConvert.GetLeanoteOptions());
			return json;
		}
		public static string ToJsonForSimple<T>(this T t)
		{
			string json = JsonSerializer.Serialize(t, MyJsonConvert.GetSimpleOptions());
			return json;
		}
		public static string ToJsonForDic(this Dictionary<string, long> t)
		{
			string json = JsonSerializer.Serialize(t, MyJsonConvert.GetSimpleOptions());
			return json;
		}

	}
}
