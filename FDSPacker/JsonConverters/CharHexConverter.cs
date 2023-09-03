using System.Text.Json.Serialization;
using System.Text.Json;

namespace wtf.cluster.FDSPacker.JsonConverters
{
    internal class CharHexConverter : JsonConverter<char>
    {
        public override char Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (value!.Length == 1)
                return value[0];
            else
                return (char)value.ParseHex();
        }

        public override void Write(Utf8JsonWriter writer, char value, JsonSerializerOptions options)
        {
            if (!char.IsControl(value) && char.IsAscii(value))
                writer.WriteStringValue(new string(value, 1));
            writer.WriteStringValue($"${(byte)value:X02}");
        }
    }
}
