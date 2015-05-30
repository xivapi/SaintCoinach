using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class SklbFile {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct HeaderData {
            public uint Magic;
            public int Unknown1;
            public short Unknown2;
            public short HavokDataOffset;
        }

        #region Properties
        public IO.File File { get; private set; }
        public HeaderData Header { get; private set; }
        public PapAnimation[] Animations { get; private set; }
        public byte[] HavokData { get; private set; }
        public byte[] Parameters { get; private set; }
        #endregion

        #region Constructor
        public SklbFile(IO.File file) {
            this.File = file;

            Build();
        }
        #endregion

        #region Build
        private void Build() {
            var buffer = File.GetData();

            var offset = 0;
            Header = buffer.ToStructure<HeaderData>(ref offset);
            if (Header.Magic != 0x736B6C62)
                throw new System.IO.InvalidDataException();

            HavokData = new byte[buffer.Length - Header.HavokDataOffset];
            Array.Copy(buffer, Header.HavokDataOffset, HavokData, 0, HavokData.Length);
        }
        #endregion
    }
}
