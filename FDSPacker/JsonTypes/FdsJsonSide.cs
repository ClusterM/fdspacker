using Newtonsoft.Json;
using System.ComponentModel;
using wtf.cluster.FDSPacker.JsonConverters;
using static com.clusterrr.Famicom.Containers.FdsBlockDiskInfo;

namespace wtf.cluster.FDSPacker.JsonTypes
{
    internal class FdsJsonSide
    {
        [JsonIgnore]
        public bool WriteUnknown { get; set; } = true;

        /// <summary>
        /// Manufacturer code. 0x00 = Unlicensed, 0x01 = Nintendo
        /// </summary>
        [JsonProperty(Order = 0)]
        [JsonConverter(typeof(EnumHexConverter<Company>))]
        public Company LicenseeCode { get; set; }

        /// <summary>
        /// 3-letter ASCII code per game (e.g. ZEL for The Legend of Zelda)
        /// </summary>
        [JsonProperty(Order = 1)]
        public string? GameName { get; set; }

        /// <summary>
        /// 0x20 = " " — Normal disk
        /// 0x45 = "E" — Event(e.g.Japanese national DiskFax tournaments)
        /// 0x52 = "R" — Reduction in price via advertising
        /// </summary>
        [JsonProperty(Order = 2)]
        [JsonConverter(typeof(CharHexConverter))]
        public char GameType { get; set; }

        /// <summary>
        /// Game version/revision number. Starts at 0x00, increments per revision
        /// </summary>
        [JsonProperty(Order = 3)]
        [JsonConverter(typeof(ByteIntConverter))]
        public byte GameVersion { get; set; }

        /// <summary>
        /// Side number. Single-sided disks use A
        /// </summary>
        [JsonProperty(Order = 4)]
        [JsonConverter(typeof(EnumHexConverter<DiskSides>))]
        public DiskSides DiskSide { get; set; }

        /// <summary>
        /// Disk number. First disk is 0x00, second is 0x01, etc.
        /// </summary>
        [JsonProperty(Order = 5)]
        [JsonConverter(typeof(ByteIntConverter))]
        public byte DiskNumber { get; set; }

        /// <summary>
        /// Disk type. 0x00 = FMC ("normal card"), 0x01 = FSC ("card with shutter"). May correlate with FMC and FSC product codes
        /// </summary>
        [JsonProperty(Order = 6)]
        [JsonConverter(typeof(EnumHexConverter<DiskTypes>))]
        public DiskTypes DiskType { get; set; }

        /// <summary>
        /// Unknown, offset 0x18.
        /// Always 0x00
        /// </summary>
        [JsonProperty(Order = 7)]
        [DefaultValue(0x00)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte Unknown01 { get; set; } = 0x00;

        /// <summary>
        /// Boot read file code. Refers to the file code/file number to load upon boot/start-up
        /// </summary>
        [JsonProperty(Order = 8)]
        [JsonConverter(typeof(ByteIntConverter))]
        public byte BootFile { get; set; }

        /// <summary>
        /// Unknown, offset 0x1A.
        /// Always 0xFF
        /// </summary>
        [JsonProperty(Order = 9)]
        [DefaultValue(0xFF)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte Unknown02 { get; set; } = 0xFF;

        /// <summary>
        /// Unknown, offset 0x1B.
        /// Always 0xFF
        /// </summary>
        [JsonProperty(Order = 10)]
        [DefaultValue(0xFF)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte Unknown03 { get; set; } = 0xFF;

        /// <summary>
        /// Unknown, offset 0x1C.
        /// Always 0xFF
        /// </summary>
        [JsonProperty(Order = 11)]
        [DefaultValue(0xFF)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte Unknown04 { get; set; } = 0xFF;

        /// <summary>
        /// Unknown, offset 0x1D.
        /// Always 0xFF
        /// </summary>
        [JsonProperty(Order = 12)]
        [DefaultValue(0xFF)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte Unknown05 { get; set; } = 0xFF;

        /// <summary>
        /// Unknown, offset 0x1E.
        /// Always 0xFF
        /// </summary>
        [JsonProperty(Order = 13)]
        [DefaultValue(0xFF)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte Unknown06 { get; set; } = 0xFF;

        /// <summary>
        /// Manufacturing date
        /// </summary>
        [JsonProperty(Order = 14)]
        public DateTime? ManufacturingDate { get; set; }

        /// <summary>
        /// Country code. 0x49 = Japan
        /// </summary>
        [JsonProperty(Order = 15)]
        [JsonConverter(typeof(EnumHexConverter<Country>))]
        public Country CountryCode { get; set; }

        /// <summary>
        /// Unknown, offset 0x23.
        /// Always 0x61.
        /// Speculative: Region code?
        /// </summary>
        [JsonProperty(Order = 16)]
        [DefaultValue(0x61)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte Unknown07 { get; set; } = 0x61;

        /// <summary>
        /// Unknown, offset 0x24.
        /// Always 0x00.
        /// Speculative: Location/site?
        /// </summary>
        [JsonProperty(Order = 17)]
        [DefaultValue(0x00)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte Unknown08 { get; set; } = 0x00;

        /// <summary>
        /// Unknown, offset 0x25.
        /// Always 0x00
        /// </summary>
        [JsonProperty(Order = 18)]
        [DefaultValue(0x00)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte Unknown09 { get; set; } = 0x00;

        /// <summary>
        /// Unknown, offset 0x26.
        /// Always 0x02
        /// </summary>
        [JsonProperty(Order = 19)]
        [DefaultValue(0x02)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte Unknown10 { get; set; } = 0x02;

        /// <summary>
        /// Unknown, offset 0x27. Speculative: some kind of game information representation?
        /// </summary>
        [JsonProperty(Order = 20)]
        [DefaultValue(0x00)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte Unknown11 { get; set; } = 0x00;

        /// <summary>
        /// Unknown, offset 0x28. Speculative: some kind of game information representation?
        /// </summary>
        [JsonProperty(Order = 21)]
        [DefaultValue(0x00)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte Unknown12 { get; set; } = 0x00;

        /// <summary>
        /// Unknown, offset 0x29. Speculative: some kind of game information representation?
        /// </summary>
        [JsonProperty(Order = 22)]
        [DefaultValue(0x00)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte Unknown13 { get; set; } = 0x00;

        /// <summary>
        /// Unknown, offset 0x2A. Speculative: some kind of game information representation?
        /// </summary>
        [JsonProperty(Order = 23)]
        [DefaultValue(0x00)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte Unknown14 { get; set; } = 0x00;

        /// <summary>
        /// Unknown, offset 0x2B. Speculative: some kind of game information representation?
        /// </summary>
        [JsonProperty(Order = 24)]
        [DefaultValue(0x00)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte Unknown15 { get; set; } = 0x00;

        /// <summary>
        /// "Rewritten disk" date. It's speculated this refers to the date the disk was formatted and rewritten by something like a Disk Writer kiosk.
        /// In the case of an original (non-copied) disk, this should be the same as Manufacturing date
        /// </summary>
        [JsonProperty(Order = 25)]
        public DateTime? RewrittenDate { get; set; }

        /// <summary>
        /// Unknown, offset 0x2F
        /// </summary>
        [JsonProperty(Order = 26)]
        [DefaultValue(0x00)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte Unknown16 { get; set; } = 0x00;

        /// <summary>
        /// Unknown, offset 0x30.
        /// Always 0x80
        /// </summary>
        [JsonProperty(Order = 27)]
        [DefaultValue(0x80)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte Unknown17 { get; set; } = 0x80;

        /// <summary>
        /// Disk Writer serial number
        /// </summary>
        [JsonProperty(Order = 28)]
        [JsonConverter(typeof(UShortHexConverter))]
        public ushort DiskWriterSerialNumber { get; set; }

        /// <summary>
        /// Unknown, offset 0x33, unknown.
        /// Always 0x07
        /// </summary>
        [JsonProperty(Order = 29)]
        [DefaultValue(0x00)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte Unknown18 { get; set; } = 0x00;

        /// <summary>
        /// Disk rewrite count.
        /// 0x00 = Original (no copies)
        /// </summary>
        [JsonProperty(Order = 30)]
        [JsonConverter(typeof(ByteIntConverter))]
        public byte DiskRewriteCount { get; set; }

        /// <summary>
        /// Actual disk side
        /// </summary>
        [JsonProperty(Order = 31)]
        [JsonConverter(typeof(EnumHexConverter<DiskSides>))]
        public DiskSides ActualDiskSide { get; set; }

        /// <summary>
        /// Disk type (other)
        /// </summary>
        [JsonProperty(Order = 32)]
        [JsonConverter(typeof(EnumHexConverter<DiskTypesOther>))]
        public DiskTypesOther DiskTypeOther { get; set; }

        /// <summary>
        /// Unknown how this differs from GameVersion. Disk version numbers indicate different software revisions.
        /// Speculation is that disk version incremented with each disk received from a licensee
        /// </summary>
        [JsonProperty(Order = 33)]
        [JsonConverter(typeof(ByteHexConverter))]
        public byte DiskVersion { get; set; }

        /// <summary>
        /// Non-hidden file amount
        /// </summary>
        [JsonProperty(Order = 34)]
        public byte FileAmount { get; set; }

        /// <summary>
        /// Files
        /// </summary>
        [JsonProperty(Order = 35)]
        public List<FdsJsonFile> Files { get; set; } = new();

        public bool ShouldSerializeUnknown01() => WriteUnknown;
        public bool ShouldSerializeUnknown02() => WriteUnknown;
        public bool ShouldSerializeUnknown03() => WriteUnknown;
        public bool ShouldSerializeUnknown04() => WriteUnknown;
        public bool ShouldSerializeUnknown05() => WriteUnknown;
        public bool ShouldSerializeUnknown06() => WriteUnknown;
        public bool ShouldSerializeUnknown07() => WriteUnknown;
        public bool ShouldSerializeUnknown08() => WriteUnknown;
        public bool ShouldSerializeUnknown09() => WriteUnknown;
        public bool ShouldSerializeUnknown10() => WriteUnknown;
        public bool ShouldSerializeUnknown11() => WriteUnknown;
        public bool ShouldSerializeUnknown12() => WriteUnknown;
        public bool ShouldSerializeUnknown13() => WriteUnknown;
        public bool ShouldSerializeUnknown14() => WriteUnknown;
        public bool ShouldSerializeUnknown15() => WriteUnknown;
        public bool ShouldSerializeUnknown16() => WriteUnknown;
        public bool ShouldSerializeUnknown17() => WriteUnknown;
        public bool ShouldSerializeUnknown18() => WriteUnknown;
    }
}
