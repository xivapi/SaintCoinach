using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Sound {
    public class ScdAdpcmEntry : ScdEntry {
        #region Fields
        private byte[] _Decoded;
        #endregion

        #region Constructor
        internal ScdAdpcmEntry(ScdFile file, ScdEntryHeader header, int chunksOffset, int dataOffset)
            : base(file, header) {
            Decode(chunksOffset, dataOffset);
        }
        #endregion

        public override byte[] GetDecoded() {
            return _Decoded;
        }

        #region Decode
        private void Decode(int chunksOffset, int dataOffset) {
            const int WaveHeaderSize = 0x10;

            var wavHeaderOffset = dataOffset;
            var finalDataOffset = chunksOffset + Header.SamplesOffset;

            _Decoded = new byte[0x1C + WaveHeaderSize + Header.DataSize];
            var o = 0;
            _Decoded[o++] = (byte)'R';
            _Decoded[o++] = (byte)'I';
            _Decoded[o++] = (byte)'F';
            _Decoded[o++] = (byte)'F';

            Array.Copy(BitConverter.GetBytes((int)(0x14 + WaveHeaderSize + Header.DataSize)), 0, _Decoded, o, 4);
            o += 4;

            _Decoded[o++] = (byte)'W';
            _Decoded[o++] = (byte)'A';
            _Decoded[o++] = (byte)'V';
            _Decoded[o++] = (byte)'E';
            _Decoded[o++] = (byte)'f';
            _Decoded[o++] = (byte)'m';
            _Decoded[o++] = (byte)'t';
            _Decoded[o++] = (byte)' ';

            Array.Copy(BitConverter.GetBytes((int)WaveHeaderSize), 0, _Decoded, o, 4);
            o += 4;

            Array.Copy(File._InputBuffer, wavHeaderOffset, _Decoded, o, WaveHeaderSize);
            o += WaveHeaderSize;

            _Decoded[o++] = (byte)'d';
            _Decoded[o++] = (byte)'a';
            _Decoded[o++] = (byte)'t';
            _Decoded[o++] = (byte)'a';

            Array.Copy(BitConverter.GetBytes((int)Header.DataSize), 0, _Decoded, o, 4);
            o += 4;
            Array.Copy(File._InputBuffer, finalDataOffset, _Decoded, o, Header.DataSize);
        }
        #endregion
    }
}
