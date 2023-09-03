using Newtonsoft.Json;

namespace wtf.cluster.FDSPacker.JsonConverters
{
    internal class ByteHexConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(byte);

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var value = reader.Value?.ToString();
            return (byte)value!.ParseHex();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            writer.WriteValue($"${value:X02}");
        }
    }
}
