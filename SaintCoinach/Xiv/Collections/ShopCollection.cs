using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.Collections {
    public class ShopCollection : IEnumerable<IShop> {
        #region Enumerator
        private class Enumerator : IEnumerator<IShop> {
            #region Fields
            private XivCollection _Collection;

            private IEnumerator<Shop> _ShopEnumerator;
            private IEnumerator<GCShop> _GCShopEnumerator;
            private IEnumerator<SpecialShop> _SpecialShopEnumerator;

            private int _State = 0;
            private IShop _Current;
            #endregion

            #region Constructor
            public Enumerator(XivCollection collection) {
                _Collection = collection;
                _ShopEnumerator = collection.GetSheet<Shop>().GetEnumerator();
                _GCShopEnumerator = collection.GetSheet<GCShop>().GetEnumerator();
                _SpecialShopEnumerator = collection.GetSheet<SpecialShop>().GetEnumerator();
            }
            #endregion

            #region IEnumerator<Item> Members
            public IShop Current {
                get { return _Current; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose() {
                _ShopEnumerator.Dispose();
                _GCShopEnumerator.Dispose();
                _SpecialShopEnumerator.Dispose();
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current {
                get { return Current; }
            }

            public bool MoveNext() {
                var result = false;

                _Current = null;
                if (_State == 0) {
                    result = _ShopEnumerator.MoveNext();
                    if (result)
                        _Current = _ShopEnumerator.Current;
                    else
                        ++_State;
                }
                if (_State == 1) {
                    result = _GCShopEnumerator.MoveNext();
                    if (result)
                        _Current = _GCShopEnumerator.Current;
                    else
                        ++_State;
                }
                if (_State == 2) {
                    result = _SpecialShopEnumerator.MoveNext();
                    if (result)
                        _Current = _SpecialShopEnumerator.Current;
                    else
                        ++_State;
                }

                return result;
            }

            public void Reset() {
                _State = 0;
                _ShopEnumerator.Reset();
                _GCShopEnumerator.Dispose();
                _SpecialShopEnumerator.Dispose();
            }

            #endregion
        }
        #endregion

        #region Fields
        private XivCollection _Collection;
        #endregion

        #region Properties
        public XivCollection Collection { get { return _Collection; } }
        #endregion

        #region Constructor
        public ShopCollection(XivCollection collection) {
            _Collection = collection;
        }
        #endregion

        #region IEnumerable<IShop> Members
        public IEnumerator<IShop> GetEnumerator() {
            return new Enumerator(Collection);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
