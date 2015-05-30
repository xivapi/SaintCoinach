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
            public uint Format;
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


            int havokOffset;
            switch (Header.Format) {
                case 0x31323030:    // 0021
                    havokOffset = BitConverter.ToInt16(buffer, 0x0A);
                    break;
                case 0x31333030:    // 0031
                    havokOffset = BitConverter.ToInt16(buffer, 0x0C);
                    break;
                default:
                    throw new NotSupportedException();
            }
            HavokData = new byte[buffer.Length - havokOffset];
            Array.Copy(buffer, havokOffset, HavokData, 0, HavokData.Length);
        }
        #endregion
    }
}
