using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Variant2 {
    internal class ColumnValues {
        internal Dictionary<int, object> Values = new Dictionary<int, object>();
        internal Dictionary<int, object> RawValues = new Dictionary<int, object>();

        public Column Column { get; private set; }
        public object this[int index] {
            get { return Values[index]; }
        }

        internal ColumnValues(Column column) {
            Column = column;
        }
    }
}
