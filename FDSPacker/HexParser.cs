using System.Globalization;

namespace wtf.cluster.FDSPacker
{
    public static class HexParser
    {
        public static ushort ParseHex(this string input)
        {
            if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                return ushort.Parse(input[2..], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            else if (input.StartsWith("$", StringComparison.OrdinalIgnoreCase))
                return ushort.Parse(input[1..], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            return ushort.Parse(input);
        }
    }
}
