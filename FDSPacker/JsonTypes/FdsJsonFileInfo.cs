using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using wtf.cluster.FDSPacker;
using static com.clusterrr.Famicom.Containers.FdsBlockFileHeader;

namespace wtf.cluster.FDSPacker.JsonTypes
{
    internal class FdsJsonFile
    {
        public byte FileNumber { get; set; }
        public byte FileIndicateCode { get; set; }
        public string? FileName { get; set; }
        public ushort FileAddress { get; set; }
        public Kind FileKind { get; set; }
        public string? Data { get; set; }
    }
}
