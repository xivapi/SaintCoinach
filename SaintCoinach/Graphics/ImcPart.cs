using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class ImcPart {
        #region Fields
        internal List<ImcVariant> _Variants = new List<ImcVariant>();
        private IReadOnlyList<ImcVariant> _ROVariants;
        #endregion

        #region Properties
        public byte Bit { get; private set; }
        public ImcVariant DefaultVariant { get { return _Variants[0]; } }
        public IReadOnlyList<ImcVariant> Variants { get { return _ROVariants ?? (_ROVariants = new ReadOnlyCollection<ImcVariant>(_Variants)); } }
        #endregion

        #region Constructor
        internal ImcPart(byte[] buffer, byte bit, ref int offset) {
            this.Bit = bit;
            _Variants.Add(buffer.ToStructure<ImcVariant>(ref offset));
        }
        #endregion
    }
}
