namespace SaintCoinach.Ex.Relational {
    public class RelationalMultiRow : MultiRow, IRelationalMultiRow {
        #region Constructors

        #region Constructor

        public RelationalMultiRow(IMultiSheet sheet, int key) : base(sheet, key) { }

        #endregion

        #endregion

        public new IRelationalMultiSheet Sheet { get { return (IRelationalMultiSheet)base.Sheet; } }

        #region IRelationalMultiRow Members

        public object this[string columnName, Language language] {
            get { return Sheet.GetLocalisedSheet(language)[Key, columnName]; }
        }

        #endregion

        public override string ToString() {
            var defCol = Sheet.Header.DefaultColumn;
            return defCol == null
                       ? string.Format("{0}#{1}", Sheet.Header.Name, Key)
                       : string.Format("{0}", this[defCol.Index]);
        }

        #region IRelationalRow Members

        IRelationalSheet IRelationalRow.Sheet { get { return Sheet; } }

        public object DefaultValue { get { return Sheet.ActiveSheet[Key].DefaultValue; } }

        public object this[string columnName] { get { return Sheet.ActiveSheet[Key, columnName]; } }

        object IRelationalRow.GetRaw(string columnName) {
            return Sheet.ActiveSheet[Key].GetRaw(columnName);
        }

        object IRelationalMultiRow.GetRaw(string columnName, Language language) {
            return Sheet.GetLocalisedSheet(language)[Key].GetRaw(columnName);
        }

        #endregion
    }
}
