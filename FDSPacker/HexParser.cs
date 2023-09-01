using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtf.cluster.FDSPacker
{
    public static class HexParser
    {
        public static ushort ParseHex(this string input)
        {
            if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                return ushort.Parse(input[2..], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            return ushort.Parse(input);
        }
    }
}
