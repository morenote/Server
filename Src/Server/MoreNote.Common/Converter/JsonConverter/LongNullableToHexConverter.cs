using MoreNote.Common.ExtensionMethods;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MoreNote.Common.Converter.JsonConverter
{
	/// <summary>
	/// long类型转Hex
	/// </summary>
	public class LongNullableToHexConverter : JsonConverter<long?>
	{
		public override long? Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options) => reader.GetString().ToLongByHex();

		public override void Write(
			Utf8JsonWriter writer,
			long? id,
			JsonSerializerOptions options) =>
				writer.WriteStringValue(id.ToHex());
	}
}
