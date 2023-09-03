using System.Text.Json.Serialization;
using System.Text.Json;

namespace wtf.cluster.FDSPacker.JsonConverters
{
    internal class UShortIntConverter : JsonConverter<ushort>
    {
        public override ushort Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return (ushort)value!.ParseHex();
        }

        public override void Write(Utf8JsonWriter writer, ushort value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($"{value}");
        }
    }
}
