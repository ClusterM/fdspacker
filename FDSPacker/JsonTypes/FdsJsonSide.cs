using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtf.cluster.FDSPacker;
using static com.clusterrr.Famicom.Containers.FdsBlockDiskInfo;

namespace wtf.cluster.FDSPacker.JsonTypes
{
    internal class FdsJsonSide
    {
        /// <summary>
        /// 3-letter ASCII code per game (e.g. ZEL for The Legend of Zelda)
        /// </summary>
        public string? GameName { get; set; } = new("---");

        private byte manufacturerCode;
        /// <summary>
        /// Manufacturer code. = = 0x00, Unlicensed, = = 0x01, Nintendo
        /// </summary>
        public string ManufacturerCode { get => $"0x{manufacturerCode:X02}"; set => manufacturerCode = (byte)value.ParseHex(); }

        /// <summary>
        /// = = 0x20, " " — Normal disk
        /// = = 0x45, "E" — Event(e.g.Japanese national DiskFax tournaments)
        /// = = 0x52, "R" — Reduction in price via advertising
        /// </summary>
        public char GameType { get; set; }

        /// <summary>
        /// Game version/revision number. Starts at $00, increments per revision
        /// </summary>
        public byte GameVersion { get; set; }

        /// <summary>
        /// Side number. Single-sided disks use A
        /// </summary>
        public DiskSides DiskSide { get; set; }

        /// <summary>
        /// Disk number. First disk is $00, second is $01, etc.
        /// </summary>
        public byte DiskNumber { get; set; }

        /// <summary>
        /// Disk type: FMC ("normal card") or FSC ("card with shutter")
        /// </summary>
        public DiskTypes DiskType { get; set; }

        /// <summary>
        /// Boot read file code. Refers to the file code/file number to load upon boot/start-up
        /// </summary>
        public byte BootFile { get; set; }

        /// <summary>
        /// Manufacturing date
        /// </summary>
        public DateTime? ManufacturingDate { get; set; }

        byte countryCode;
        /// <summary>
        /// Country code. 0x49 = Japan
        /// </summary>
        public string CountryCode { get => $"0x{countryCode:X02}"; set => countryCode = (byte)value.ParseHex(); }

        /// <summary>
        /// "Rewritten disk" date. It's speculated this refers to the date the disk was formatted and rewritten by something like a Disk Writer kiosk.
        /// In the case of an original (non-copied) disk, this should be the same as Manufacturing date
        /// </summary>
        public DateTime? RewrittenDate { get; set; }

        private ushort diskWriterSerialNumber;
        /// <summary>
        /// Disk Writer serial number
        /// </summary>
        public string DiskWriterSerialNumber { get => $"0x{diskWriterSerialNumber:X04}"; set => diskWriterSerialNumber = value.ParseHex(); }

        /// <summary>
        /// Disk rewrite count. 0x00 = Original (no copies)
        /// </summary>
        public byte DiskRewriteCount { get; set; }

        /// <summary>
        /// Actual disk side
        /// </summary>
        public DiskSides ActualDiskSide { get; set; }

        /// <summary>
        /// Price code
        /// </summary>
        public byte Price { get; set; }

        /// <summary>
        /// Visible amount of files
        /// </summary>
        public byte FileAmount { get; set; }

        /// <summary>
        /// Files
        /// </summary>
        public List<FdsJsonFile> Files { get; set; } = new();
    }
}
