using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.Collections {
    public class ENpcCollection : IEnumerable<ENpc> {
        #region Enumerator
        private class Enumerator : IEnumerator<ENpc> {
            #region Fields
            private ENpc _Current;
            private ENpcCollection _Collection;
            private IEnumerator<ENpcBase> _BaseEnumerator;
            #endregion

            #region Constructor
            public Enumerator(ENpcCollection collection) {
                _Collection = collection;
                _BaseEnumerator = collection.BaseSheet.GetEnumerator();
            }
            #endregion

            #region IEnumerator<ENpc> Members
            public ENpc Current {
                get { return _Current; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose() {
                _BaseEnumerator.Dispose();
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current {
                get { return Current; }
            }

            public bool MoveNext() {
                bool result = _BaseEnumerator.MoveNext();
                if (result)
                    _Current = _Collection.Get(_BaseEnumerator.Current.Key);
                else
                    _Current = null;
                return result;
            }

            public void Reset() {
                _Current = null;
                _BaseEnumerator.Reset();
            }

            #endregion
        }
        #endregion

        #region Fields
        private XivCollection _Collection;
        private IXivSheet<ENpcBase> _BaseSheet;
        private IXivSheet<ENpcResident> _ResidentSheet;

        private Dictionary<int, ENpc> _Inner = new Dictionary<int, ENpc>();
        #endregion

        #region Properties
        public XivCollection Collection { get { return _Collection; } }
        public IXivSheet<ENpcBase> BaseSheet { get { return _BaseSheet; } }
        public IXivSheet<ENpcResident> ResidentSheet { get { return _ResidentSheet; } }
        #endregion

        #region Constructor
        public ENpcCollection(XivCollection xivCollection) {
            _Collection = xivCollection;
            _BaseSheet = xivCollection.GetSheet<ENpcBase>();
            _ResidentSheet = xivCollection.GetSheet<ENpcResident>();
        }
        #endregion

        #region Get
        public ENpc Get(int key) {
            if (_Inner.ContainsKey(key))
                return _Inner[key];

            var enpc = new ENpc(this, key);
            _Inner.Add(key, enpc);
            return enpc;
        }
        #endregion

        #region Find
        public IEnumerable<ENpc> FindWithData(int value) {
            foreach (var npc in this) {
                for (var i = 0; i < ENpcBase.DataCount; ++i) {
                    var val = npc.Base.GetData(i);
                    if (val == value)
                        yield return npc;
                }
            }
        }
        #endregion

        #region IEnumerable<ENpc> Members

        public IEnumerator<ENpc> GetEnumerator() {
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
