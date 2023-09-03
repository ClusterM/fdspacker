using Newtonsoft.Json;

namespace wtf.cluster.FDSPacker.JsonConverters
{
    internal class CharHexConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(char);

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var value = reader.Value?.ToString();
            if (value!.Length == 1)
                return value[0];
            else
                return (char)value.ParseHex();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (!char.IsControl((char)value!) && char.IsAscii((char)value))
                writer.WriteValue(new string((char)value, 1));
            else
                writer.WriteValue($"${Convert.ToByte(value):X02}");
        }
    }
}
