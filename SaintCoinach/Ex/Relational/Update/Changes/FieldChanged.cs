using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational.Update.Changes {
    [Serializable]
    public class FieldChanged : IChange {
        private string _SheetName;
        private string _ColumnName;
        private int _Row;
        private object _PreviousValue;
        private object _UpdatedValue;
        private Language _Language;

        public ChangeType ChangeType { get { return ChangeType.Data; } }
        public string SheetName { get { return _SheetName; } }
        public string ColumnName { get { return _ColumnName; } }
        public int Row { get { return _Row; } }
        public object PreviousValue { get { return _PreviousValue; } }
        public object UpdatedValue { get { return _UpdatedValue; } }
        public Language Language { get { return _Language; } }

        public FieldChanged(string sheetName, Language language, string columnName, int row, object previousVal, object updatedValue) {
            _SheetName = sheetName;
            _ColumnName = columnName;
            _Row = row;
            _Language = language;
            _PreviousValue = previousVal;
            _UpdatedValue = updatedValue;
        }

        public override string ToString() {
            return string.Format("'{0}':{1}{5}[{2}]: Changed from '{3}' to '{4}'.", SheetName, Row, ColumnName, PreviousValue, UpdatedValue, Language.GetSuffix());
        }
    }
}
