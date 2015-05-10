using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class ImcFile {
        #region Fields
        private Dictionary<byte, ImcPart> _Parts = new Dictionary<byte, ImcPart>();
        #endregion

        #region Properties
        public IO.File SourceFile { get; private set; }
        public IEnumerable<ImcPart> Parts { get { return _Parts.Values; } }
        public short PartsMask { get; private set; }
        public short Count { get; private set; }
        #endregion

        #region Constructor
        public ImcFile(IO.File sourceFile) {
            this.SourceFile = sourceFile;

            var buffer = SourceFile.GetData();
            this.Count = BitConverter.ToInt16(buffer, 0);
            this.PartsMask  = BitConverter.ToInt16(buffer, 2);

            var offset = 4;
            for (byte bit = 0; bit < 8; ++bit) {
                var match = 1 << bit;
                if ((PartsMask & match) == match)
                    _Parts.Add(bit, new ImcPart(buffer, bit, ref offset));
            }

            var rem = Count;
            while (--rem >= 0) {
                foreach (var part in _Parts.Values)
                    part._Variants.Add(buffer.ToStructure<ImcVariant>(ref offset));
            }
        }
        #endregion

        #region Get
        public ImcVariant GetVariant(int index) {
            return Parts.First().Variants[index];
        }
        public ImcVariant GetVariant(byte partKey, int index) {
            return _Parts[partKey].Variants[index];
        }
        #endregion
    }
}
