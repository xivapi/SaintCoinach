using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Lgb {
    public class LgbFile {
        #region Struct
        [StructLayout(LayoutKind.Sequential)]
        public struct HeaderData {
            public uint Magic1;     // LGB1
            public uint FileSize;
            public uint Unknown1;
            public uint Magic2;     // LGP1
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public int GroupCount;
        }
        #endregion

        #region Properties
        public HeaderData Header { get; private set; }
        public IO.File File { get; private set; }
        public LgbGroup[] Groups { get; private set; }
        #endregion

        #region Constructor
        public LgbFile(IO.File file) {
            this.File = file;

            Build();
        }
        #endregion

        #region Build
        private void Build() {
            var buffer = File.GetData();

            var baseOffset = 0;

            this.Header = buffer.ToStructure<HeaderData>(ref baseOffset);
            if (Header.Magic1 != 0x3142474C || Header.Magic2 != 0x3150474C)     // LGB1 & LGP1
                throw new System.IO.InvalidDataException();

            Groups = new LgbGroup[Header.GroupCount];
            for (int i = 0; i < Header.GroupCount; ++i) {
                var groupOffset = baseOffset + BitConverter.ToInt32(buffer, baseOffset + i * 4);
                Groups[i] = new LgbGroup(this, buffer, groupOffset);
            }
        }
        #endregion
    }
}
