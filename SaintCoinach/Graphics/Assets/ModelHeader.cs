using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Assets {
    public class ModelHeader {
        #region Fields
        internal string[] _Strings;
        internal byte[] _Buffer;
        internal byte[] _FixedData;
        internal string[] _AttributeNames;
        internal string[] _BoneNames;
        internal Bone[] _Bones;
        internal string[] _MaterialNames;
        private int _SubModelCount = 3;     // XXX: Not sure if this is stored anywhere, or it's always three, or both.
        private int _MeshCount;
        private ModelQuality[] _AvailableQualities = new ModelQuality[] { ModelQuality.High };  // Just going to go with this for now.
        internal MeshHeader[] _MeshHeaders;
        internal SubModelHeader[] _SubModelHeaders;
        #endregion

        #region Properties
        public int AttributeCount { get { return _AttributeNames.Length; } }
        public int BoneCount { get { return _BoneNames.Length; } }
        public int MaterialCount { get { return _MaterialNames.Length; } }
        public int SubModelCount { get { return _SubModelCount; } }
        public int MeshCount { get { return _MeshCount; } }
        public IEnumerable<ModelQuality> AvailableQualities { get { return _AvailableQualities; } }
        public string GetAttributeName(int index) { return _AttributeNames[index]; }
        public string GetBoneName(int index) { return _BoneNames[index]; }
        public Bone GetBone(int index) { return _Bones[index]; }
        public string GetMaterialName(int index) { return _MaterialNames[index]; }
        public MeshHeader GetMeshHeader(int index) { return _MeshHeaders[index]; }
        public SubModelHeader GetSubModelHeader(int index) { return _SubModelHeaders[index]; }
        public SubModelHeader GetSubModelHeader(ModelQuality quality) { return GetSubModelHeader((int)quality); }
        public byte[] GetBuffer() {
            var b = new byte[_Buffer.Length];
            Array.Copy(_Buffer, b, b.Length);
            return b;
        }
        #endregion

        #region Constructor
        public ModelHeader(byte[] buffer) {
            _Buffer = buffer;

            Read();
        }
        private void Read() {
            const int FixedDataLength = 0x38;
            const int MeshCountOffset = 0x04;

            int offset;

            _Strings = ReadStrings(out offset);

            _FixedData = new byte[FixedDataLength];
            Array.Copy(_Buffer, offset, _FixedData, 0, FixedDataLength);
            offset += FixedDataLength;

            _MeshCount = BitConverter.ToUInt16(_FixedData, MeshCountOffset);

            AssignAttributeNames();
            AssignBoneNames();
            AssignMaterialNames();

            SkipToModelHeaders(ref offset);

            ReadSubModelHeaders(ref offset);
            ReadMeshHeaders(ref offset);

            SkipToBones(ref offset);

            ReadBones(ref offset);
        }
        private string[] ReadStrings(out int offset) {
            const int CountOffset = 0x00;
            const int LengthOffset = 0x04;
            const int StringsOffset = 0x08;

            var count = BitConverter.ToInt32(_Buffer, CountOffset);
            var length = BitConverter.ToInt32(_Buffer, LengthOffset);
            var end = StringsOffset + length;

            offset = StringsOffset;
            var strings = new string[count];
            for (int i = 0; i < count; ++i) {
                if (offset >= end)
                    throw new System.IO.InvalidDataException();

                var strEnd = offset - 1;
                while (++strEnd < end && _Buffer[strEnd] != 0) ;
                var len = strEnd - offset;

                strings[i] = Encoding.ASCII.GetString(_Buffer, offset, len);

                offset = strEnd + 1;
            }

            offset = StringsOffset + length;

            return strings;
        }
        private void AssignAttributeNames() {
            const int CountOffset = 0x06;

            var count = BitConverter.ToUInt16(_FixedData, CountOffset);
            _AttributeNames = new string[count];
            for (int i = 0; i < count; ++i)
                _AttributeNames[i] = _Strings[i];
        }
        private void AssignBoneNames() {
            const int CountOffset = 0x0C;

            var d = AttributeCount;

            var count = BitConverter.ToUInt16(_FixedData, CountOffset);
            _BoneNames = new string[count];
            for (int i = 0; i < count; ++i)
                _BoneNames[i] = _Strings[i + d];
        }
        private void AssignMaterialNames() {
            const int CountOffset = 0x0A;

            var d = AttributeCount + BoneCount;

            var count = BitConverter.ToUInt16(_FixedData, CountOffset);
            _MaterialNames = new string[count];
            // XXX: Sometimes it goes past the last string...
            for (int i = 0; i < count && i + d < _Strings.Length; ++i)
                _MaterialNames[i] = _Strings[i + d];

            if (count + d - 1 >= _Strings.Length)
                System.Diagnostics.Trace.WriteLine("ModelHeader: Indicated material past available date.");
        }

        private void SkipToModelHeaders(ref int offset) {
            const int CountOffset = 0x18;
            const int Length = 4 * 8;

            var count = BitConverter.ToUInt16(_FixedData, CountOffset);
            offset += count * Length;
        }

        private void ReadSubModelHeaders(ref int offset) {
            _SubModelHeaders = new SubModelHeader[SubModelCount];
            for (int i = 0; i < SubModelCount; ++i) {
                _SubModelHeaders[i] = new SubModelHeader(_Buffer, offset);
                offset += SubModelHeader.Size;
            }
        }
        private void ReadMeshHeaders(ref int offset) {
            _MeshHeaders = new MeshHeader[MeshCount];
            for (int i = 0; i < MeshCount; ++i) {
                var name = string.Format("Mesh#{0}", i);
                if (i < AttributeCount)
                    name = GetAttributeName(i);
                _MeshHeaders[i] = new MeshHeader(name, _Buffer, offset);
                offset += MeshHeader.Size;
            }
        }
        private void SkipToBones(ref int offset) {
            offset += 0x04 * AttributeCount;
            offset += 0x14 * _FixedData[0x1A];                          // Just a byte?
            offset += 0x10 * BitConverter.ToInt16(_FixedData, 0x08);    // Something about indices?

            // XXX: Not sure about this block D:
            {
                offset += 0x0C * BitConverter.ToInt16(_FixedData, 0x26);    // Newp, possibly not in this order
                offset += 0x04 * MaterialCount;
                offset += 0x04 * BoneCount;
            }

            offset += 0x84 * BitConverter.ToInt16(_FixedData, 0x0E);    // Something about models?
            offset += 0x10 * BitConverter.ToInt16(_FixedData, 0x10);    // Newp
            offset += 0x0C * BitConverter.ToInt16(_FixedData, 0x12);    // Newp
            offset += 0x04 * BitConverter.ToInt16(_FixedData, 0x14);    // Newp

            offset += BitConverter.ToInt32(_Buffer, offset) + 4;        // Newp
            
            offset += _Buffer[offset] + 1;                              // Merely padding

            offset += 0x80;                                             // Dunno what this is
        }
        private void ReadBones(ref int offset) {
            _Bones = new Bone[BoneCount];
            for (int i = 0; i < BoneCount; ++i) {
                var b = new byte[0x20];
                Array.Copy(_Buffer, offset, b, 0, b.Length);

                _Bones[i] = new Bone(GetBoneName(i), b);

                offset += b.Length;
            }
        }
        #endregion
    }
}
