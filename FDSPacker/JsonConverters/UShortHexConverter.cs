using Newtonsoft.Json;

namespace wtf.cluster.FDSPacker.JsonConverters
{
    internal class UShortHexConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(ushort);

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var value = reader.Value?.ToString();
            return value!.ParseHex();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            writer.WriteValue($"${value:X04}");
        }
    }
}
