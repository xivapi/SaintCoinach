using System.Collections;
using System.Collections.Generic;

namespace SaintCoinach.Xiv.Collections {
    public class ItemCollection : IEnumerable<ItemBase> {
        #region Fields

        private readonly Range[] _EventRanges;
        private readonly Range[] _InventoryRanges;

        #endregion

        #region Properties

        public XivCollection Collection { get; private set; }
        public IXivSheet<Item> InventoryItemSheet { get; private set; }
        public IXivSheet<EventItem> EventItemSheet { get; private set; }

        #endregion

        #region Constructors

        public ItemCollection(XivCollection collection) {
            Collection = collection;

            InventoryItemSheet = collection.GetSheet<Item>();
            EventItemSheet = collection.GetSheet<EventItem>();

            _InventoryRanges = Range.Combine(InventoryItemSheet.Header.DataFileRanges);
            _EventRanges = Range.Combine(EventItemSheet.Header.DataFileRanges);
        }

        #endregion

        #region IEnumerable<Item> Members

        public IEnumerator<ItemBase> GetEnumerator() {
            return new Enumerator(this);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region Enumerator

        private class Enumerator : IEnumerator<ItemBase> {
            #region Fields

            private readonly IEnumerator<EventItem> _EventItemEnumerator;
            private readonly IEnumerator<Item> _InventoryItemEnumerator;
            private int _State;

            #endregion

            #region Constructors

            #region Constructor

            public Enumerator(ItemCollection collection) {
                _InventoryItemEnumerator = collection.InventoryItemSheet.GetEnumerator();
                _EventItemEnumerator = collection.EventItemSheet.GetEnumerator();
            }

            #endregion

            #endregion

            #region IEnumerator<Item> Members

            public ItemBase Current { get; private set; }

            #endregion

            #region IDisposable Members

            public void Dispose() {
                _InventoryItemEnumerator.Dispose();
                _EventItemEnumerator.Dispose();
            }

            #endregion

            #region IEnumerator Members

            object IEnumerator.Current { get { return Current; } }

            public bool MoveNext() {
                var result = false;

                Current = null;
                if (_State == 0) {
                    result = _InventoryItemEnumerator.MoveNext();
                    if (result)
                        Current = _InventoryItemEnumerator.Current;
                    else
                        ++_State;
                }
                // ReSharper disable once InvertIf
                if (_State == 1) {
                    result = _EventItemEnumerator.MoveNext();
                    if (result)
                        Current = _EventItemEnumerator.Current;
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

        #region Things

        public ItemBase this[int index] {
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
    }
}
