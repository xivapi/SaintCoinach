using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class PapFile {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct HeaderData {
            public uint Magic;
            public short Unknown1;
            public short Unknown2;
            public short AnimationCount;
            public short Unknown3;
            public short Unknown4;
            public short Unknown5;
            public short Unknown6;
            public int HavokDataOffset;
            public int ParametersOffset;
        }

        #region Properties
        public IO.File File { get; private set; }
        public HeaderData Header { get; private set; }
        public PapAnimation[] Animations { get; private set; }
        public byte[] HavokData { get; private set; }
        public byte[] Parameters { get; private set; }
        #endregion

        #region Constructor
        public PapFile(IO.File file) {
            this.File = file;

            Build();
        }
        #endregion

        #region Build
        private void Build() {
            var buffer = File.GetData();

            var offset = 0;
            Header = buffer.ToStructure<HeaderData>(ref offset);
            if (Header.Magic != 0x20706170)
                throw new System.IO.InvalidDataException();

            Animations = new PapAnimation[Header.AnimationCount];
            for (var i = 0; i < Header.AnimationCount; ++i)
                Animations[i] = new PapAnimation(this, buffer, ref offset);

            HavokData = new byte[Header.ParametersOffset - Header.HavokDataOffset];
            Array.Copy(buffer, Header.HavokDataOffset, HavokData, 0, HavokData.Length);

            Parameters = new byte[buffer.Length - Header.ParametersOffset];
            Array.Copy(buffer, Header.ParametersOffset, Parameters, 0, Parameters.Length);
        }
        #endregion
    }
}
