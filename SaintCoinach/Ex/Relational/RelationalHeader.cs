using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Relational {
    public class RelationalHeader : Header {
        #region Fields
        private RelationalColumn[] _Columns;
        #endregion

        #region Properties
        public new RelationalExCollection Collection { get { return (RelationalExCollection)base.Collection; } }
        public new IEnumerable<RelationalColumn> Columns { get { return _Columns; } }
        public RelationalColumn DefaultColumn {
            get {
                var def = SheetDefinition;
                if (def != null) {
                    var i = def.GetDefaultColumnIndex();
                    if (i.HasValue)
                        return GetColumn(i.Value);
                }
                return null;
            }
            set {
                var def = GetOrCreateSheetDefinition();
                if (value == null)
                    def.DefaultColumn = null;
                else
                    def.DefaultColumn = value.Name;
            }
        }

        public new RelationalColumn GetColumn(int index) { return _Columns[index]; }

        public Definition.SheetDefinition SheetDefinition {
            get {
                Definition.SheetDefinition def;
                if (Collection.Definition.TryGetSheet(Name, out def))
                    return def;
                return null;
            }
        }
        public Definition.SheetDefinition GetOrCreateSheetDefinition() {
            return Collection.Definition.GetOrCreateSheet(Name);
        }
        #endregion

        #region Constructor
        public RelationalHeader(RelationalExCollection collection, string name, IO.File file)
            : base(collection, name, file) {

            _Columns = base.Columns.Cast<RelationalColumn>().ToArray();
        }
        #endregion

        #region Factory
        protected override Column CreateColumn(int index, byte[] data, int offset) {
            return new RelationalColumn(this, index, data, offset);
        }
        #endregion

        #region Helpers
        public RelationalColumn FindColumn(string name) {
            var def = SheetDefinition;
            if (def != null) {
                var i = def.FindColumn(name);
                if (i.HasValue)
                    return GetColumn(i.Value);
            }
            return null;
        }
        #endregion
    }
}
