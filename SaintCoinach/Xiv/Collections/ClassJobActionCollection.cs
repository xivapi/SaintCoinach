using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.Collections {
    public class ClassJobActionCollection : IEnumerable<ClassJobActionBase> {
        #region Fields

        private readonly Range[] _ActionRanges;
        private readonly Range[] _CraftActionRanges;

        #endregion

        #region Properties

        public XivCollection Collection { get; private set; }
        public IXivSheet<Action> ActionSheet { get; private set; }
        public IXivSheet<CraftAction> CraftActionSheet { get; private set; }

        #endregion

        #region Constructors

        public ClassJobActionCollection(XivCollection collection) {
            Collection = collection;

            ActionSheet = collection.GetSheet<Action>();
            CraftActionSheet = collection.GetSheet<CraftAction>();

            _ActionRanges = Range.Combine(ActionSheet.Header.DataFileRanges);
            _CraftActionRanges = Range.Combine(CraftActionSheet.Header.DataFileRanges);
        }

        #endregion

        #region IEnumerable<ClassJobActionBase> Members

        public IEnumerator<ClassJobActionBase> GetEnumerator() {
            return new Enumerator(this);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region Enumerator
        class Enumerator : IEnumerator<ClassJobActionBase> {
            #region Fields

            private readonly IEnumerator<Action> _ActionEnumerator;
            private readonly IEnumerator<CraftAction> _CraftActionEnumerator;
            private int _State;

            #endregion

            #region Constructors

            #region Constructor
            
            public Enumerator(ClassJobActionCollection collection) {
                _ActionEnumerator = collection.ActionSheet.GetEnumerator();
                _CraftActionEnumerator = collection.CraftActionSheet.GetEnumerator();
            }

            #endregion

            #endregion

            #region IEnumerator<Item> Members

            public ClassJobActionBase Current { get; private set; }

            #endregion

            #region IDisposable Members

            public void Dispose() {
                _ActionEnumerator.Dispose();
                _CraftActionEnumerator.Dispose();
            }

            #endregion

            #region IEnumerator Members

            object IEnumerator.Current { get { return Current; } }

            public bool MoveNext() {
                var result = false;

                Current = null;
                if (_State == 0) {
                    result = _ActionEnumerator.MoveNext();
                    if (result)
                        Current = _ActionEnumerator.Current;
                    else
                        ++_State;
                }
                // ReSharper disable once InvertIf
                if (_State == 1) {
                    result = _CraftActionEnumerator.MoveNext();
                    if (result)
                        Current = _CraftActionEnumerator.Current;
                    else
                        ++_State;
                }

                return result;
            }

            public void Reset() {
                _State = 0;
                _ActionEnumerator.Reset();
                _CraftActionEnumerator.Reset();
            }

            #endregion
        }
        #endregion


        #region Things

        public ClassJobActionBase this[int index] {
            get {
                if (Range.Contains(_ActionRanges, index))
                    return ActionSheet[index];
                if (Range.Contains(_CraftActionRanges, index))
                    return CraftActionSheet[index];
                throw new KeyNotFoundException();
            }
        }

        public bool ContainsRow(int index) {
            if (Range.Contains(_ActionRanges, index))
                return ActionSheet.ContainsRow(index);
            if (Range.Contains(_CraftActionRanges, index))
                return CraftActionSheet.ContainsRow(index);
            return false;
        }

        #endregion

    }
}
