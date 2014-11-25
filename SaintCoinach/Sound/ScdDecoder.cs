using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Sound {
    public class ScdDecoder {
        #region Fields
        private byte[] _FileIn;
        private byte[] _FileOut;

        private bool _UseLittleEndian = false;

        private int _VorbHeaderOffset;
        private int _VorbHeaderLength;
        private byte _Xor;
        #endregion

        #region Properties
        public byte[] GetData() { return _FileOut; }
        #endregion

        #region Constructor
        public ScdDecoder(byte[] fileIn) {
            _FileIn = fileIn;
            _FileOut = new byte[_FileIn.Length];
            Array.Copy(_FileIn, _FileOut, _FileIn.Length);

            Decode();
        }
        #endregion

        #region Decode
        private void Decode() {
            Init();
            ParseMeta();
            Unscramble();
        }
        private void ParseMeta() {
            var metaOffsetOffset = 0x40 + ReadInt16(0x0E);
            var metaOffset = ReadInt32(metaOffsetOffset);

            var codecId = ReadInt32(metaOffset + 0x0C);
            if (codecId != 0x06)    // OGG
                throw new NotSupportedException(string.Format("Codec {0:X2}h not supported.", codecId));

            var postMetaOffset = metaOffset + 0x20;

            var auxChunkCount = ReadInt32(metaOffset + 0x1C);
            while (auxChunkCount-- > 0)
                postMetaOffset += ReadInt32(postMetaOffset + 4);

            var seekTableSize = ReadInt32(postMetaOffset + 0x10);

            _Xor = _FileIn[postMetaOffset + 0x02];
            _VorbHeaderOffset = postMetaOffset + 0x20 + seekTableSize;
            _VorbHeaderLength = ReadInt32(postMetaOffset + 0x14);
        }
        private void Unscramble() {
            if (_Xor == 0)
                return;
            for (int i = 0; i < _VorbHeaderLength; ++i)
                _FileOut[_VorbHeaderOffset + i] ^= _Xor;
        }
        private void Init() {
            // Check magic (SEDBSSCF)
            if (ReadInt64(0, false) != 0x5345444253534346)
                throw new InvalidDataException();

            // Check endianness
            var verBE = ReadInt32(8, false);
            var verLE = ReadInt32(8, true);

            if (verBE == 2 || verBE == 3) {
                _UseLittleEndian = false;
            } else if (verLE == 2 || verLE == 3) {
                _UseLittleEndian = true;
            } else
                throw new InvalidDataException();
        }
        #endregion

        #region Helpers
        private short ReadInt16(int offset) {
            return ReadInt16(offset, _UseLittleEndian);
        }
        private int ReadInt32(int offset) {
            return ReadInt32(offset, _UseLittleEndian);
        }
        private long ReadInt64(int offset) {
            return ReadInt64(offset, _UseLittleEndian);
        }
        private short ReadInt16(int offset, bool littleEndian) {
            var buffer = new byte[2];

            Array.Copy(_FileIn, offset, buffer, 0, buffer.Length);
            if (BitConverter.IsLittleEndian != littleEndian)
                Array.Reverse(buffer);

            return BitConverter.ToInt16(buffer, 0);
        }
        private int ReadInt32(int offset, bool littleEndian) {
            var buffer = new byte[4];

            Array.Copy(_FileIn, offset, buffer, 0, buffer.Length);
            if (BitConverter.IsLittleEndian != littleEndian)
                Array.Reverse(buffer);

            return BitConverter.ToInt32(buffer, 0);
        }
        private long ReadInt64(int offset, bool littleEndian) {
            var buffer = new byte[8];

            Array.Copy(_FileIn, offset, buffer, 0, buffer.Length);
            if (BitConverter.IsLittleEndian != littleEndian)
                Array.Reverse(buffer);

            return BitConverter.ToInt64(buffer, 0);
        }
        #endregion
    }
}
