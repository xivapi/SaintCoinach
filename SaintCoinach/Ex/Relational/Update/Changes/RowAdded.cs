using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Update.Changes {
    public class RowAdded : IChange {
        private string _SheetName;
        private int _Row;

        public ChangeType ChangeType { get { return ChangeType.Data; } }
        public string SheetName { get { return _SheetName; } }
        public int Row { get { return _Row; } }

        public RowAdded(string sheetName, int row) {
            _SheetName = sheetName;
            _Row = row;
        }

        public override string ToString() {
            return string.Format("Row {0} was added to {1}.", Row, SheetName);
        }
    }
}
