using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Sound {
    public class ScdFile {
        #region Fields
        private bool _UseLittleEndian;
        internal byte[] _InputBuffer;
        #endregion

        #region Properties
        public IO.File SourceFile { get; private set; }
        public ScdHeader ScdHeader { get; private set; }
        public ScdEntryHeader[] EntryHeaders { get; private set; }
        public ScdEntry[] Entries { get; private set; }
        #endregion

        #region Constructor
        public ScdFile(IO.File sourceFile) {
            if (sourceFile == null)
                throw new ArgumentNullException("sourceFile");
            this.SourceFile = sourceFile;

            Decode();
        }
        #endregion

        #region Decode
        private void Decode() {
            _InputBuffer = SourceFile.GetData();

            Init();

            var fileHeaderSize = ReadInt16(0x0E);

            ReadScdHeader(fileHeaderSize);
            
            var entryHeaders = new ScdEntryHeader[ScdHeader.EntryCount];
            var entryChunkOffsets = new int[ScdHeader.EntryCount];
            var entryDataOffsets = new int[ScdHeader.EntryCount];
            for (var i = 0; i < ScdHeader.EntryCount; ++i) {
                var headerOffset = ReadInt32(ScdHeader.EntryTableOffset + 4 * i);
                entryHeaders[i] = ReadEntryHeader(headerOffset);

                entryChunkOffsets[i] = headerOffset + System.Runtime.InteropServices.Marshal.SizeOf<ScdEntryHeader>();
                entryDataOffsets[i] = entryChunkOffsets[i];
                for (var j = 0; j < entryHeaders[i].AuxChunkCount; ++j)
                    entryDataOffsets[i] += ReadInt32(entryDataOffsets[i] + 4);
            }

            this.Entries = new ScdEntry[ScdHeader.EntryCount];
            for (var i = 0; i < ScdHeader.EntryCount; ++i)
                this.Entries[i] = CreateEntry(entryHeaders[i], entryChunkOffsets[i], entryDataOffsets[i]);

            _InputBuffer = null;
        }
        private void Init() {
            // Check magic (SEDBSSCF)
            if (ReadInt64(0, false) != 0x5345444253534346)
                throw new InvalidDataException();

            // Check endianness
            var verBigEndian = ReadInt32(8, false);
            var verLittleEndian = ReadInt32(8, true);

            if (verBigEndian == 2 || verBigEndian == 3) {
                _UseLittleEndian = false;
            } else if (verLittleEndian == 2 || verLittleEndian == 3) {
                _UseLittleEndian = true;
            } else
                throw new InvalidDataException();
        }
        private void ReadScdHeader(int offset) {
            var h = new ScdHeader();

            h.Unknown1Count = ReadInt16(offset + 0x00);
            h.Unknown2Count = ReadInt16(offset + 0x02);
            h.EntryCount = ReadInt16(offset + 0x04);
            h.Unknown1 = ReadInt16(offset + 0x06);
            h.Unknown1Offset = ReadInt32(offset + 0x08);
            h.EntryTableOffset = ReadInt32(offset + 0x0C);
            h.Unknown2Offset = ReadInt32(offset + 0x10);
            h.Unknown2 = ReadInt32(offset + 0x14);
            h.UnknownOffset1 = ReadInt32(offset + 0x18);

            this.ScdHeader = h;
        }
        private ScdEntryHeader ReadEntryHeader(int offset) {
            var h = new ScdEntryHeader();

            h.DataSize = ReadInt32(offset + 0x00);
            h.ChannelCount = ReadInt32(offset + 0x04);
            h.Frequency = ReadInt32(offset + 0x08);
            h.Codec = (ScdCodec)ReadInt32(offset + 0x0C);
            h.LoopStartSample = ReadInt32(offset + 0x10);
            h.LoopEndSample = ReadInt32(offset + 0x14);
            h.SamplesOffset = ReadInt32(offset + 0x18);
            h.AuxChunkCount = ReadInt16(offset + 0x1C);
            h.Unknown1 = ReadInt16(offset + 0x1E);

            return h;
        }
        private ScdEntry CreateEntry(ScdEntryHeader header, int chunksOffset, int dataOffset) {
            if (header.DataSize == 0 || header.Codec == ScdCodec.None)
                return null;

            switch (header.Codec) {
                case ScdCodec.OGG:
                    return new ScdOggEntry(this, header, dataOffset);
                case ScdCodec.MSADPCM:
                    return new ScdAdpcmEntry(this, header, chunksOffset, dataOffset);
                default:
                    throw new NotSupportedException();
            }
        }
        #endregion

        #region Helpers

        internal short ReadInt16(int offset) {
            return ReadInt16(offset, _UseLittleEndian);
        }

        internal int ReadInt32(int offset) {
            return ReadInt32(offset, _UseLittleEndian);
        }

        internal long ReadInt64(int offset) {
            return ReadInt64(offset, _UseLittleEndian);
        }

        internal short ReadInt16(int offset, bool littleEndian) {
            var buffer = new byte[2];

            Array.Copy(_InputBuffer, offset, buffer, 0, buffer.Length);
            if (BitConverter.IsLittleEndian != littleEndian)
                Array.Reverse(buffer);

            return BitConverter.ToInt16(buffer, 0);
        }

        internal int ReadInt32(int offset, bool littleEndian) {
            var buffer = new byte[4];

            Array.Copy(_InputBuffer, offset, buffer, 0, buffer.Length);
            if (BitConverter.IsLittleEndian != littleEndian)
                Array.Reverse(buffer);

            return BitConverter.ToInt32(buffer, 0);
        }

        internal long ReadInt64(int offset, bool littleEndian) {
            var buffer = new byte[8];

            Array.Copy(_InputBuffer, offset, buffer, 0, buffer.Length);
            if (BitConverter.IsLittleEndian != littleEndian)
                Array.Reverse(buffer);

            return BitConverter.ToInt64(buffer, 0);
        }

        #endregion
    }
}
