using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Variant2 {
    public class SubRow {
        internal Dictionary<int, object> Values = new Dictionary<int, object>();
        internal Dictionary<int, object> RawValues = new Dictionary<int, object>();

        public int Key { get; private set; }
        public IDataRow ParentRow { get; private set; }
        public object this[int column] {
            get { return Values[column]; }
        }
        public object GetRaw(int column) {
            return RawValues[column];
        }

        internal SubRow(IDataRow parent, int key) {
            ParentRow = parent;
            Key = key;
        }
    }
}
