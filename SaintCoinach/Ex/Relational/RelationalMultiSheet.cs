using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational {
    public class RelationalMultiSheet<TMulti, TData> : MultiSheet<TMulti, TData>, IRelationalMultiSheet<TMulti, TData>
        where TMulti : IRelationalMultiRow
        where TData : IRelationalDataRow {

        #region Properties
        public new RelationalExCollection Collection { get { return (RelationalExCollection)base.Collection; } }
        public new RelationalHeader Header { get { return (RelationalHeader)base.Header; } }
        public new IRelationalSheet<TData> ActiveSheet { get { return (IRelationalSheet<TData>)base.ActiveSheet; } }
        #endregion

        #region Constructor
        public RelationalMultiSheet(RelationalExCollection collection, RelationalHeader header) : base(collection, header) { }
        #endregion

        #region Factory
        protected override ISheet<TData> CreateLocalisedSheet(Language language) {
            return new RelationalDataSheet<TData>(Collection, Header, language);
        }
        #endregion

        #region IRelationalMultiSheet<TMulti,TSource> Members
        public new IRelationalSheet<TData> GetLocalisedSheet(Language language) {
            return (IRelationalSheet<TData>)base.GetLocalisedSheet(language);
        }
        #endregion

        #region IRelationalSheet Members
        IEnumerable<IRelationalRow> IRelationalSheet.GetAllRows() {
            return base.GetAllRows().Cast<IRelationalRow>();
        }

        IRelationalRow IRelationalSheet.this[int row] {
            get { return this[row]; }
        }

        object IRelationalSheet.this[int row, string columnName] {
            get { return this[row][columnName]; }
        }
        #endregion

        #region IRelationalMultiSheet Members

        IRelationalSheet IRelationalMultiSheet.ActiveSheet {
            get { return ActiveSheet; }
        }

        IRelationalSheet IRelationalMultiSheet.GetLocalisedSheet(Language language) {
            return GetLocalisedSheet(language);
        }

        IEnumerable<IRelationalMultiRow> IRelationalMultiSheet.GetAllRows() {
            return GetAllRows().Cast<IRelationalMultiRow>();
        }

        IRelationalMultiRow IRelationalMultiSheet.this[int row] {
            get { return this[row]; }
        }

        #endregion
    }
}
