using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational {
    public class RelationalColumn : Column {
        #region Properties
        public new RelationalHeader Header { get { return (RelationalHeader)base.Header; } }
        public string Name {
            get {
                var def = Header.SheetDefinition;
                if (def != null)
                    return def.GetColumnName(Index);
                return null;
            }
        }
        public override string ValueType {
            get {
                var def = Header.SheetDefinition;
                if (def != null) {
                    var t = def.GetValueType(Index);
                    if (t != null)
                        return t;
                }
                return base.ValueType;
            }
        }
        #endregion

        #region Constructor
        public RelationalColumn(RelationalHeader header, int index, byte[] buffer, int offset) : base(header, index, buffer, offset) { }
        #endregion

        #region Read
        public override object Read(byte[] buffer, IDataRow row) {
            var baseVal = base.Read(buffer, row);

            var def = Header.SheetDefinition;
            if (def != null)
                return def.Convert(row, baseVal, Index);
            return baseVal;
        }
        #endregion

        public override string ToString() {
            var n = Name;
            if (n == null)
                return Index.ToString();
            return n;
        }
    }
}
