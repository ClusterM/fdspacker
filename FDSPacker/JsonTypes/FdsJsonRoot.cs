using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtf.cluster.FDSPacker.JsonTypes
{
    internal class FdsJsonRoot
    {
        public List<FdsJsonSide> Sides { get; set; } = new();
    }
}
