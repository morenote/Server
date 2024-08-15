using MoreNote.Common.ExtensionMethods;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MoreNote.Common.Converter.JsonConverter
{
	public class LeanoteLongJsonConvert : JsonConverter<long>
	{

		public override bool HandleNull => true;
		public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			//return long.Parse(reader.GetString(), System.Globalization.NumberStyles.HexNumber);
			return reader.GetString().ToLongByHex().Value;
		}

		public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
		{
			if (value == null)
			{
				writer.WriteStringValue(string.Empty);
			}
			else
			{
				writer.WriteStringValue(value.ToHex24ForLeanote());
			}

		}
	}
}
