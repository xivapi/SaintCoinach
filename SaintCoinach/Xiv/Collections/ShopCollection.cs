using System.Collections;
using System.Collections.Generic;

namespace SaintCoinach.Xiv.Collections {
    public class ShopCollection : IEnumerable<IShop> {
        #region Properties

        public XivCollection Collection { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public ShopCollection(XivCollection collection) {
            Collection = collection;
        }

        #endregion

        #endregion

        #region IEnumerable<IShop> Members

        public IEnumerator<IShop> GetEnumerator() {
            return new Enumerator(Collection);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region Enumerator

        private class Enumerator : IEnumerator<IShop> {
            #region Fields

            // ReSharper disable once InconsistentNaming
            private readonly IEnumerator<GCShop> _GCShopEnumerator;
            private readonly IEnumerator<GilShop> _GilShopEnumerator;
            private readonly IEnumerator<SpecialShop> _SpecialShopEnumerator;
            private readonly IEnumerator<FccShop> _FccShopEnumerator;
            private int _State;

            #endregion

            #region Constructors

            #region Constructor

            public Enumerator(XivCollection collection) {
                _GilShopEnumerator = collection.GetSheet<GilShop>().GetEnumerator();
                _GCShopEnumerator = collection.GetSheet<GCShop>().GetEnumerator();
                _SpecialShopEnumerator = collection.GetSheet<SpecialShop>().GetEnumerator();
                _FccShopEnumerator = collection.GetSheet<FccShop>().GetEnumerator();
            }

            #endregion

            #endregion

            #region IEnumerator<Item> Members

            public IShop Current { get; private set; }

            #endregion

            #region IDisposable Members

            public void Dispose() {
                _GilShopEnumerator.Dispose();
                _GCShopEnumerator.Dispose();
                _SpecialShopEnumerator.Dispose();
            }

            #endregion

            #region IEnumerator Members

            object IEnumerator.Current { get { return Current; } }

            public bool MoveNext() {
                var result = false;

                Current = null;
                if (_State == 0) {
                    result = _GilShopEnumerator.MoveNext();
                    if (result)
                        Current = _GilShopEnumerator.Current;
                    else
                        ++_State;
                }
                if (_State == 1) {
                    result = _GCShopEnumerator.MoveNext();
                    if (result)
                        Current = _GCShopEnumerator.Current;
                    else
                        ++_State;
                }
                // ReSharper disable once InvertIf
                if (_State == 2) {
                    result = _SpecialShopEnumerator.MoveNext();
                    if (result)
                        Current = _SpecialShopEnumerator.Current;
                    else
                        ++_State;
                }

                if(_State == 3) {
                    result = _FccShopEnumerator.MoveNext();
                    if (result)
                        Current = _FccShopEnumerator.Current;
                    else
                        ++_State;
                }

                return result;
            }

            public void Reset() {
                _State = 0;
                _GilShopEnumerator.Reset();
                _GCShopEnumerator.Dispose();
                _SpecialShopEnumerator.Dispose();
            }

            #endregion
        }

        #endregion
    }
}
