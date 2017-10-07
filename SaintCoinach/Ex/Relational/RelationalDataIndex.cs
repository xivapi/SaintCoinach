using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational {
    public class RelationalDataIndex<T> where T : IDataRow {
        #region Fields

        private Dictionary<int, T> _IndexedRows;

        #endregion

        #region Properties

        public IDataSheet<T> SourceSheet { get; private set; }
        public Column IndexColumn { get; private set; }

        #endregion


        public RelationalDataIndex (IDataSheet<T> sourceSheet, Column indexColumn) {
            SourceSheet = sourceSheet;
            IndexColumn = indexColumn;

            BuildIndex();
        }

        private void BuildIndex() {
            _IndexedRows = new Dictionary<int, T>();

            var index = IndexColumn.Index;

            foreach (var row in SourceSheet) {
                var value = row.GetRaw(index);
                _IndexedRows[Convert.ToInt32(value)] = row;
            }
        }

        public T this[int key] {
            get {
                return _IndexedRows.TryGetValue(key, out var row) ? row : default(T);
            }
        }
    }
}
