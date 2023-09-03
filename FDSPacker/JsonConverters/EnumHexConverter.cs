using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace wtf.cluster.FDSPacker.JsonConverters
{
    internal class EnumHexConverter<T> : JsonConverter<T> where T : Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (string.IsNullOrEmpty(value))
                return (T)Enum.ToObject(typeToConvert, 0);
            if (Enum.TryParse(typeof(T), value, true, out object? result))
                return (T)result!;
            return (T)Enum.ToObject(typeToConvert, value.ParseHex());
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (Enum.IsDefined(typeof(T), value))
            {
                writer.WriteStringValue(value.ToString());
            }
            else
            {
                writer.WriteStringValue($"${Convert.ToUInt32(value):X02}");
            }
        }
    }
}
