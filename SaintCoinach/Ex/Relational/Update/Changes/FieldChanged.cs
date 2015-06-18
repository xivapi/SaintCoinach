using System;

namespace SaintCoinach.Ex.Relational.Update.Changes {
    [Serializable]
    public class FieldChanged : IChange {
        #region Properties

        public string ColumnName { get; private set; }
        public int Row { get; private set; }
        public object PreviousValue { get; private set; }
        public object UpdatedValue { get; private set; }
        public Language Language { get; private set; }

        #endregion

        #region Constructors

        public FieldChanged(string sheetName,
                            Language language,
                            string columnName,
                            int row,
                            object previousVal,
                            object updatedValue) {
            SheetName = sheetName;
            ColumnName = columnName;
            Row = row;
            Language = language;

            if (previousVal is Text.XivString)
                PreviousValue = previousVal.ToString();
            else
                PreviousValue = previousVal;

            if (updatedValue is Text.XivString)
                UpdatedValue = updatedValue.ToString();
            else
                UpdatedValue = updatedValue;
        }

        #endregion

        public ChangeType ChangeType { get { return ChangeType.Data; } }
        public string SheetName { get; private set; }

        public override string ToString() {
            return string.Format("'{0}':{1}{5}[{2}]: Changed from '{3}' to '{4}'.", SheetName, Row, ColumnName,
                PreviousValue, UpdatedValue, Language.GetSuffix());
        }
    }
}
