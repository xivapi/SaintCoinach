using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SaintCoinach.Ex;
using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public partial class XivSheet2<T> : XivSheet<XivRow>, IEnumerable where T : IXivSubRow {
        #region Fields

        private ConstructorInfo _SubRowConstructor;
        private Dictionary<Tuple<int, int>, T> _SubRows = new Dictionary<Tuple<int, int>, T>();

        #endregion

        #region Properties

        private ConstructorInfo SubRowConstructor {
            get {
                if (_SubRowConstructor != null) return _SubRowConstructor;
                return _SubRowConstructor = GetRowConstructor(typeof(T), GetType());
            }
        }

        #endregion

        #region Constructors

        public XivSheet2(XivCollection collection, IRelationalSheet source):
            base(collection, source) { }

        #endregion

        #region IEnumerable<T> Members

        public new IEnumerator<T> GetEnumerator() {
            return new Enumerator(this);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Factory

        protected virtual T CreateSubRow(IRelationalRow sourceRow) {
            if (SubRowConstructor == null)
                throw new NotSupportedException("No matching constructor found.");

            return (T)SubRowConstructor.Invoke(new object[] {
                this, sourceRow
            });
        }

        #endregion
    }
}
