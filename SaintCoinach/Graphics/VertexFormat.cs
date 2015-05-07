using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class VertexFormat {
        #region Properties
        public VertexFormatElement[] Elements { get; private set; }
        #endregion

        #region Constructor
        internal VertexFormat(byte[] buffer, ref int offset) {
            var o = offset;

            var elements = new List<VertexFormatElement>();
            while (buffer[o] != 0xFF)
                elements.Add(buffer.ToStructure<VertexFormatElement>(ref o));
            this.Elements = elements.ToArray();

            offset += 0x88;
        }
        #endregion
    }
}
