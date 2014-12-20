using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Update.Changes {
    [Serializable]
    public class RowRemoved : IChange {
        private string _SheetName;
        private int _Row;

        public ChangeType ChangeType { get { return ChangeType.Data; } }
        public string SheetName { get { return _SheetName; } }
        public int Row { get { return _Row; } }

        public RowRemoved(string sheetName, int row) {
            _SheetName = sheetName;
            _Row = row;
        }

        public override string ToString() {
            return string.Format("Row {0} was removed from {1}.", Row, SheetName);
        }
    }
}
