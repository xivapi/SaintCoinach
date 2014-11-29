using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.Collections {
    public class ItemCollection : IEnumerable<Item> {
        #region Enumerator
        private class Enumerator : IEnumerator<Item> {
            #region Fields
            private ItemCollection _Collection;
            private IEnumerator<Item> _InventoryItemEnumerator;
            private IEnumerator<EventItem> _EventItemEnumerator;
            private int _State = 0;
            private Item _Current;
            #endregion

            #region Constructor
            public Enumerator(ItemCollection collection) {
                _Collection = collection;
                _InventoryItemEnumerator = collection.InventoryItemSheet.GetEnumerator();
                _EventItemEnumerator = collection.EventItemSheet.GetEnumerator();
            }
            #endregion

            #region IEnumerator<Item> Members
            public Item Current {
                get { return _Current; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose() {
                _InventoryItemEnumerator.Dispose();
                _EventItemEnumerator.Dispose();
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
                    result = _InventoryItemEnumerator.MoveNext();
                    if (result)
                        _Current = _InventoryItemEnumerator.Current;
                    else
                        ++_State;
                }
                if (_State == 1) {
                    result = _EventItemEnumerator.MoveNext();
                    if (result)
                        _Current = _EventItemEnumerator.Current;
                    else
                        ++_State;
                }

                return result;
            }

            public void Reset() {
                _State = 0;
                _InventoryItemEnumerator.Reset();
                _EventItemEnumerator.Reset();
            }

            #endregion
        }
        #endregion

        #region Fields
        private XivCollection _Collection;
        private IXivSheet<Item> _InventoryItemSheet;
        private IXivSheet<EventItem> _EventItemSheet;
        private Range[] _InventoryRanges;
        private Range[] _EventRanges;
        #endregion

        #region Properties
        public XivCollection Collection { get { return _Collection; } }
        public IXivSheet<Item> InventoryItemSheet { get { return _InventoryItemSheet; } }
        public IXivSheet<EventItem> EventItemSheet { get { return _EventItemSheet; } }
        #endregion

        #region Constructor
        public ItemCollection(XivCollection collection) {
            _Collection = collection;

            _InventoryItemSheet = collection.GetSheet<Item>();
            _EventItemSheet = collection.GetSheet<EventItem>();

            _InventoryRanges = Range.Combine(InventoryItemSheet.Header.DataFileRanges);
            _EventRanges = Range.Combine(EventItemSheet.Header.DataFileRanges);
        }
        #endregion

        #region Things
        public Item this[int index] {
            get {
                if (Range.Contains(_InventoryRanges, index))
                    return InventoryItemSheet[index];
                if (Range.Contains(_EventRanges, index))
                    return EventItemSheet[index];
                throw new KeyNotFoundException();
            }
        }
        public bool ContainsRow(int index) {
            if (Range.Contains(_InventoryRanges, index))
                return InventoryItemSheet.ContainsRow(index);
            if (Range.Contains(_EventRanges, index))
                return EventItemSheet.ContainsRow(index);
            return false;
        }
        #endregion

        #region IEnumerable<Item> Members

        public IEnumerator<Item> GetEnumerator() {
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
