using Oje.Infrastructure.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Oje.Infrastructure.Services.ModelBinder
{
    public class JsonMyHtmlConverter : JsonConverter<MyHtmlString>
    {
        public override MyHtmlString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new MyHtmlString(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, MyHtmlString value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.innerText);
        }
    }
}
