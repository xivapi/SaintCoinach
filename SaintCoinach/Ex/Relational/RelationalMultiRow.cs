using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational {
    public class RelationalMultiRow : MultiRow, IRelationalMultiRow {
        #region Properties
        public new IRelationalMultiSheet Sheet { get { return (IRelationalMultiSheet)base.Sheet; } }
        #endregion

        #region Constructor
        public RelationalMultiRow(IMultiSheet sheet, int key) : base(sheet, key) { }
        #endregion

        #region IRelationalMultiRow Members
        public object this[string columnName, Language language] {
            get { return Sheet.GetLocalisedSheet(language)[Key, columnName]; }
        }
        #endregion

        #region IRelationalRow Members

        IRelationalSheet IRelationalRow.Sheet {
            get { return Sheet; }
        }

        public object DefaultValue {
            get { return Sheet.ActiveSheet[Key].DefaultValue; }
        }

        public object this[string columnName] {
            get { return Sheet.ActiveSheet[Key, columnName]; }
        }

        #endregion

        public override string ToString() {
            var defCol = Sheet.Header.DefaultColumn;
            if (defCol == null)
                return string.Format("{0}#{1}", Sheet.Header.Name, Key);
            return string.Format("{0}", this[defCol.Index]);
        }
    }
}
