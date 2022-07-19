using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace Oje.Infrastructure.Services.ModelBinder
{
    public class JsonTextConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(HttpUtility.HtmlEncode(value + ""));
        }
    }
}
