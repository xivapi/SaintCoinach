using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.Collections {
    public class BNpcCollection : IEnumerable<BNpc> {
        #region Fields
        private Dictionary<long, BNpc> _InnerDictionary = new Dictionary<long, BNpc>();
        private IEnumerable<Libra.BNpcName> _LibraEntries;
        #endregion

        #region Properties
        public XivCollection Collection { get; private set; }
        public IXivSheet<BNpcBase> BaseSheet { get; private set; }
        public IXivSheet<BNpcName> NameSheet { get; private set; }
        #endregion

        #region Constructor
        public BNpcCollection(XivCollection collection) {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (!collection.IsLibraAvailable)
                throw new NotSupportedException("BNpcs are only available when Libra Eorzea database is present.");
            Collection = collection;
            BaseSheet = collection.GetSheet<BNpcBase>();
            NameSheet = collection.GetSheet<BNpcName>();
        }
        #endregion

        #region Things
        public BNpc this[long key] { get { return GetOrCreate(key); } }
        #endregion

        #region Create
        private BNpc GetOrCreate(long key) {
            BNpc npc;
            if (!_InnerDictionary.TryGetValue(key, out npc))
                npc = Create(key);
            return npc;
        }
        private BNpc Create(long key) {
            return Create(Collection.Libra.BNpcNames.First(i => i.Key == key));
        }
        private BNpc GetOrCreate(Libra.BNpcName libra) {
            BNpc npc;
            if (!_InnerDictionary.TryGetValue(libra.Key, out npc))
                npc = Create(libra);
            return npc;
        }
        private BNpc Create(Libra.BNpcName libra) {
            return new BNpc(this, libra);
        }
        #endregion

        #region Enumerator
        private class Enumerator : IEnumerator<BNpc> {
            #region Fields
            private IEnumerator<Libra.BNpcName> _LibraEnumerator;
            private BNpcCollection _Collection;
            #endregion

            #region Constructor
            public Enumerator(BNpcCollection collection) {
                _Collection = collection;
                if (collection._LibraEntries == null)
                    collection._LibraEntries = collection.Collection.Libra.BNpcNames.ToArray();
                _LibraEnumerator = collection._LibraEntries.GetEnumerator();
            }
            #endregion

            #region IEnumerator<BNpc> Members

            public BNpc Current {
                get { return _Collection.GetOrCreate(_LibraEnumerator.Current); }
            }

            #endregion

            #region IDisposable Members

            public void Dispose() {
                try {
                    if (_LibraEnumerator != null)
                        _LibraEnumerator.Dispose();
                } finally { _LibraEnumerator = null; }
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current {
                get { return Current; }
            }

            public bool MoveNext() {
                return _LibraEnumerator.MoveNext();
            }

            public void Reset() {
                _LibraEnumerator.Reset();
            }

            #endregion
        }
        #endregion

        #region IEnumerable<BNpc> Members

        public IEnumerator<BNpc> GetEnumerator() {
            return new Enumerator(this);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
