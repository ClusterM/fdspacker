using Newtonsoft.Json;

namespace wtf.cluster.FDSPacker.JsonConverters
{
    internal class EnumHexConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType.IsEnum;

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var value = reader.Value?.ToString();
            if (Enum.TryParse(typeof(T), value, true, out object? result))
                return (T)result!;
            return (T)Enum.ToObject(typeof(T), value!.ParseHex());
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (Enum.IsDefined(typeof(T), value!))
                writer.WriteValue(value!.ToString());
            else
                writer.WriteValue($"${Convert.ToUInt32(value):X02}");
        }
    }
}
