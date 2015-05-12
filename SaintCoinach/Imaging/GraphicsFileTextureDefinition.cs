using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Imaging {
    // TODO: This name is what I think the .gfd extension and gftd identifier may stand for.
    public class GraphicsFileTextureDefinition : IEnumerable<GftdEntry> {
        #region Fields
        private Dictionary<short, GftdEntry> _Entries = new Dictionary<short, GftdEntry>();
        private ReadOnlyDictionary<short, GftdEntry> _ROEntries;
        #endregion

        #region Properties
        public IO.File File { get; private set; }
        public IReadOnlyDictionary<short, GftdEntry> Entries { get { return _ROEntries ?? (_ROEntries = new ReadOnlyDictionary<short, GftdEntry>(_Entries)); } }
        #endregion

        #region Constructor
        public GraphicsFileTextureDefinition(IO.File file) {
            this.File = file;

            var buffer = file.GetData();
            if (BitConverter.ToUInt64(buffer, 0) != 0x3030313064746667) // gftd0100
                throw new ArgumentException();

            var count = BitConverter.ToInt32(buffer, 8);

            var offset = 0x10;
            while (--count >= 0) {
                var entry = buffer.ToStructure<GftdEntry>(ref offset);
                _Entries.Add(entry.Id, entry);
            }
        }
        #endregion

        #region IEnumerable<GftdEntry> Members

        IEnumerator<GftdEntry> IEnumerable<GftdEntry>.GetEnumerator() {
            return _Entries.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return _Entries.Values.GetEnumerator();
        }

        #endregion
    }
}
