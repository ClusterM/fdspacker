using System.Text.Json.Serialization;
using System.Text.Json;

namespace wtf.cluster.FDSPacker.JsonConverters
{
    internal class ByteIntConverter : JsonConverter<byte>
    {
        public override byte Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return (byte)value!.ParseHex();
        }

        public override void Write(Utf8JsonWriter writer, byte value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($"{value}");
        }
    }
}
