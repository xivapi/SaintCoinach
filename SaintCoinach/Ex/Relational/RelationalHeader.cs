using System.Collections.Generic;
using System.Linq;

using SaintCoinach.Ex.Relational.Definition;
using SaintCoinach.IO;

namespace SaintCoinach.Ex.Relational {
    public class RelationalHeader : Header {
        #region Fields

        private readonly RelationalColumn[] _Columns;

        #endregion

        #region Properties

        public new RelationalExCollection Collection { get { return (RelationalExCollection)base.Collection; } }
        public new IEnumerable<RelationalColumn> Columns { get { return _Columns; } }

        public RelationalColumn DefaultColumn {
            get {
                var def = SheetDefinition;
                if (def == null) return null;

                var i = def.GetDefaultColumnIndex();
                return i.HasValue ? GetColumn(i.Value) : null;
            }
            set {
                var def = GetOrCreateSheetDefinition();
                def.DefaultColumn = value == null ? null : value.Name;
            }
        }

        public SheetDefinition SheetDefinition {
            get {
                SheetDefinition def;
                return Collection.Definition.TryGetSheet(Name, out def) ? def : null;
            }
        }

        #endregion

        #region Constructors

        #region Constructor

        public RelationalHeader(RelationalExCollection collection, string name, File file)
            : base(collection, name, file) {
            _Columns = base.Columns.Cast<RelationalColumn>().ToArray();
        }

        #endregion

        #endregion

        public new RelationalColumn GetColumn(int index) {
            return _Columns[index];
        }

        public SheetDefinition GetOrCreateSheetDefinition() {
            return Collection.Definition.GetOrCreateSheet(Name);
        }

        #region Factory

        protected override Column CreateColumn(int index, byte[] data, int offset) {
            return new RelationalColumn(this, index, data, offset);
        }

        #endregion

        #region Helpers

        public RelationalColumn FindColumn(string name) {
            var def = SheetDefinition;
            if (def == null) return null;

            var i = def.FindColumn(name);
            return i.HasValue ? GetColumn(i.Value) : null;
        }

        #endregion
    }
}
