using Newtonsoft.Json;
using wtf.cluster.FDSPacker.JsonConverters;
using static com.clusterrr.Famicom.Containers.FdsBlockFileHeader;

namespace wtf.cluster.FDSPacker.JsonTypes
{
    internal class FdsJsonFile
    {
        [JsonProperty(Order = 0)]
        [JsonConverter(typeof(ByteIntConverter))]
        public byte FileNumber { get; set; }

        [JsonProperty(Order = 1)]
        [JsonConverter(typeof(ByteIntConverter))]
        public byte FileIndicateCode { get; set; }

        [JsonProperty(Order = 2)]
        public string? FileName { get; set; }

        [JsonProperty(Order = 3)]
        [JsonConverter(typeof(UShortHexConverter))]
        public ushort FileAddress { get; set; }

        [JsonProperty(Order = 4)]
        [JsonConverter(typeof(EnumHexConverter<Kind>))]
        public Kind FileKind { get; set; }

        [JsonProperty(Order = 5)]
        public string Data { get; set; } = string.Empty;
    }
}
