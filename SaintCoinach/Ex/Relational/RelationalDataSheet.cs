using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using SaintCoinach.IO;

namespace SaintCoinach.Ex.Relational {
    public class RelationalDataSheet<T> : DataSheet<T>, IRelationalDataSheet<T> where T : IRelationalDataRow {
        #region Fields

        private ConcurrentDictionary<string, RelationalDataIndex<T>> _indexes = new ConcurrentDictionary<string, RelationalDataIndex<T>>();

        #endregion


        #region Constructors

        public RelationalDataSheet(RelationalExCollection collection, RelationalHeader header, Language language)
            : base(collection, header, language) { }

        #endregion

        public new RelationalExCollection Collection { get { return (RelationalExCollection)base.Collection; } }
        public new RelationalHeader Header { get { return (RelationalHeader)base.Header; } }

        #region Factory

        protected override ISheet<T> CreatePartialSheet(Range range, File file) {
            return new RelationalPartialDataSheet<T>(this, range, file);
        }

        #endregion

        #region IRelationalSheet Members

        IRelationalRow IRelationalSheet.this[int row] { get { return this[row]; } }

        public object this[int row, string columnName] { get { return this[row][columnName]; } }

        public IRelationalRow IndexedLookup(string indexName, int key) {
            if (key == 0)
                return null;

            var index = _indexes.GetOrAdd(indexName, i => {
                var column = Header.FindColumn(indexName);
                if (column == null)
                    throw new KeyNotFoundException();

                return new RelationalDataIndex<T>(this, column);
            });
            return index[key];
        }

        #endregion
    }
}
