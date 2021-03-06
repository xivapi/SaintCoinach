using System;

namespace SaintCoinach.Ex.Relational.Update.Changes {
    [Serializable]
    public class RowAdded : IChange {
        #region Properties

        public int Row { get; private set; }

        #endregion

        #region Constructors

        public RowAdded(string sheetName, int row) {
            SheetName = sheetName;
            Row = row;
        }

        #endregion

        public ChangeType ChangeType { get { return ChangeType.Data; } }
        public string SheetName { get; private set; }

        public override string ToString() {
            return string.Format("Row {0} was added to {1}.", Row, SheetName);
        }
    }
}
