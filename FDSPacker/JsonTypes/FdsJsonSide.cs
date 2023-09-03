using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using wtf.cluster.FDSPacker;
using wtf.cluster.FDSPacker.JsonConverters;
using static com.clusterrr.Famicom.Containers.FdsBlockDiskInfo;
using static com.clusterrr.Famicom.Containers.FdsBlockFileHeader;

namespace wtf.cluster.FDSPacker.JsonTypes
{
    internal class FdsJsonSide
    {
        [JsonPropertyOrder(0)]
        /// <summary>
        /// 3-letter ASCII code per game (e.g. ZEL for The Legend of Zelda)
        /// </summary>
        public string? GameName { get; set; } = new("---");

        [JsonPropertyOrder(1)]
        [JsonConverter(typeof(EnumHexConverter<Company>))]
        /// <summary>
        /// Manufacturer code. 0x00 = Unlicensed, 0x01 = Nintendo
        /// </summary>
        public Company LicenseeCode { get; set; }

        [JsonPropertyOrder(2)]
        [JsonConverter(typeof(CharHexConverter))]
        /// <summary>
        /// 0x20, " " = Normal disk
        /// 0x45, "E" = Event(e.g.Japanese national DiskFax tournaments)
        /// 0x52, "R" = Reduction in price via advertising
        /// </summary>
        public char GameType { get; set; }

        [JsonPropertyOrder(3)]
        [JsonConverter(typeof(ByteIntConverter))]
        /// <summary>
        /// Game version/revision number. Starts at $00, increments per revision
        /// </summary>
        public byte GameVersion { get; set; }

        [JsonPropertyOrder(4)]
        [JsonConverter(typeof(EnumHexConverter<DiskSides>))]
        /// <summary>
        /// Side number. Single-sided disks use A
        /// </summary>
        public DiskSides DiskSide { get; set; }

        [JsonPropertyOrder(5)]
        [JsonConverter(typeof(ByteIntConverter))]
        /// <summary>
        /// Disk number. First disk is $00, second is $01, etc.
        /// </summary>
        public byte DiskNumber { get; set; }

        [JsonPropertyOrder(6)]
        [JsonConverter(typeof(EnumHexConverter<DiskTypes>))]
        /// <summary>
        /// Disk type: FMC ("normal card") or FSC ("card with shutter")
        /// </summary>
        public DiskTypes DiskType { get; set; }

        [JsonPropertyOrder(7)]
        [JsonConverter(typeof(ByteIntConverter))]
        /// <summary>
        /// Boot read file code. Refers to the file code/file number to load upon boot/start-up
        /// </summary>
        public byte BootFile { get; set; }

        [JsonPropertyOrder(8)]
        /// <summary>
        /// Manufacturing date
        /// </summary>
        public DateTime? ManufacturingDate { get; set; }

        [JsonPropertyOrder(9)]
        [JsonConverter(typeof(EnumHexConverter<Country>))]
        /// <summary>
        /// Country code. 0x49 = Japan
        /// </summary>
        public Country CountryCode { get; set; }

        [JsonPropertyOrder(10)]
        /// <summary>
        /// "Rewritten disk" date. It's speculated this refers to the date the disk was formatted and rewritten by something like a Disk Writer kiosk.
        /// In the case of an original (non-copied) disk, this should be the same as Manufacturing date
        /// </summary>
        public DateTime? RewrittenDate { get; set; }

        [JsonPropertyOrder(11)]
        [JsonConverter(typeof(UShortHexConverter))]
        /// <summary>
        /// Disk Writer serial number
        /// </summary>
        public ushort DiskWriterSerialNumber { get; set; }

        [JsonPropertyOrder(12)]
        [JsonConverter(typeof(ByteIntConverter))]
        /// <summary>
        /// Disk rewrite count. 0x00 = Original (no copies)
        /// </summary>
        public byte DiskRewriteCount { get; set; }

        [JsonPropertyOrder(13)]
        [JsonConverter(typeof(EnumHexConverter<DiskSides>))]
        /// <summary>
        /// Actual disk side
        /// </summary>
        public DiskSides ActualDiskSide { get; set; }

        [JsonPropertyOrder(14)]
        [JsonPropertyName("price_code")]
        [JsonConverter(typeof(ByteHexConverter))]
        /// <summary>
        /// Price code
        /// </summary>
        public byte Price { get; set; }

        [JsonPropertyOrder(15)]
        [JsonConverter(typeof(ByteIntConverter))]
        /// <summary>
        /// Visible amount of files
        /// </summary>
        public byte FileAmount { get; set; }

        [JsonPropertyOrder(16)]
        /// <summary>
        /// Files
        /// </summary>
        public List<FdsJsonFile> Files { get; set; } = new();
    }
}
