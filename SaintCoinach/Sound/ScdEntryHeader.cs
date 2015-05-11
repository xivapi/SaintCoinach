using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Sound {
    [StructLayout(LayoutKind.Sequential)]
    public struct ScdEntryHeader {
        public int DataSize;
        public int ChannelCount;
        public int Frequency;
        public ScdCodec Codec;
        public int LoopStartSample;
        public int LoopEndSample;
        public int SamplesOffset;
        public short AuxChunkCount;
        public short Unknown1;
    }
}
