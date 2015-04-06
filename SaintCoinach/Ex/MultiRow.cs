namespace SaintCoinach.Ex {
    public class MultiRow : IMultiRow {
        #region Constructors

        #region Constructor

        public MultiRow(IMultiSheet sheet, int key) {
            Sheet = sheet;
            Key = key;
        }

        #endregion

        #endregion

        public IMultiSheet Sheet { get; private set; }
        ISheet IRow.Sheet { get { return Sheet; } }
        public int Key { get; private set; }

        #region IMultiRow Members

        public object this[int columnIndex, Language language] {
            get { return Sheet.GetLocalisedSheet(language)[Key, columnIndex]; }
        }

        public object this[int columnIndex] { get { return Sheet.ActiveSheet[Key, columnIndex]; } }

        object IRow.GetRaw(int columnIndex) {
            return Sheet.ActiveSheet[Key].GetRaw(columnIndex);
        }

        object IMultiRow.GetRaw(int columnIndex, Language language) {
            return Sheet.GetLocalisedSheet(language)[Key].GetRaw(columnIndex);
        }

        #endregion
    }
}
