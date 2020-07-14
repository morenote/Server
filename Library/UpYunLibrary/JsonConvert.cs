using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UpYunLibrary
{
    public class JsonConvert
    {
        public static System.Text.Json.JsonSerializerOptions GetOptions()
        {
            var options = new System.Text.Json.JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            options.Converters.Add(new DateTimeConverter());
            options.Converters.Add(new UserIdConverter());
            return options;
        }
        public static System.Text.Json.JsonSerializerOptions GetSimpleOptions()
        {
            var options = new System.Text.Json.JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
          
            return options;
        }

    }
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTime));
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
           // writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
            //writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz"));
            writer.WriteStringValue(value.ToLocalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz"));
        }
    }
    public class UserIdConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {

            //Debug.Assert(typeToConvert == typeof(DateTime));
            //return DateTime.Parse(reader.GetString());
            //  Debug.Assert();
            string a = reader.GetString();
            try
            {
                //return long.Parse(reader.GetString());
                return long.Parse(reader.GetString(), System.Globalization.NumberStyles.HexNumber);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }
        private static Dictionary<string, long> keyValuePairs = new Dictionary<string, long>();

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            if (value==0)
            {
                writer.WriteStringValue("");

            }
            else
            {
                writer.WriteStringValue(value.ToString("x"));

            }
           
        }

    }
}
