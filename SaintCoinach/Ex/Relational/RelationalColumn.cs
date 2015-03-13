namespace SaintCoinach.Ex.Relational {
    public class RelationalColumn : Column {
        #region Properties

        public new RelationalHeader Header { get { return (RelationalHeader)base.Header; } }

        public string Name {
            get {
                var def = Header.SheetDefinition;
                return def != null ? def.GetColumnName(Index) : null;
            }
        }

        public override string ValueType {
            get {
                var def = Header.SheetDefinition;
                if (def == null) return base.ValueType;

                var t = def.GetValueTypeName(Index);
                return t ?? base.ValueType;
            }
        }

        #endregion

        #region Constructors

        #region Constructor

        public RelationalColumn(RelationalHeader header, int index, byte[] buffer, int offset)
            : base(header, index, buffer, offset) { }

        #endregion

        #endregion

        #region Read

        public override object Read(byte[] buffer, IDataRow row) {
            var baseVal = base.Read(buffer, row);

            var def = Header.SheetDefinition;
            return def != null ? def.Convert(row, baseVal, Index) : baseVal;
        }

        #endregion

        public override string ToString() {
            return Name ?? Index.ToString();
        }
    }
}
