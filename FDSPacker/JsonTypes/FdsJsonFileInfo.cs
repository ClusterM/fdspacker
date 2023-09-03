using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using wtf.cluster.FDSPacker;
using wtf.cluster.FDSPacker.JsonConverters;
using static com.clusterrr.Famicom.Containers.FdsBlockFileHeader;

namespace wtf.cluster.FDSPacker.JsonTypes
{
    internal class FdsJsonFile
    {
        [JsonConverter(typeof(ByteIntConverter))]
        public byte FileNumber { get; set; }
        [JsonConverter(typeof(ByteIntConverter))]
        public byte FileIndicateCode { get; set; }
        public string? FileName { get; set; }
        [JsonConverter(typeof(UShortHexConverter))]
        public ushort FileAddress { get; set; }
        [JsonConverter(typeof(EnumHexConverter<Kind>))]
        public Kind FileKind { get; set; }
        public string Data { get; set; } = string.Empty;
    }
}
