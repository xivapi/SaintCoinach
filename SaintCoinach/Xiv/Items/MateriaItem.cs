using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.Items {
    public class MateriaItem : InventoryItem {
        #region Fields
        private bool _IsBuilt = false;
        private BaseParam _BaseParam;
        private int _Value;
        #endregion

        #region Constructor
        public MateriaItem(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        private void Build() {
            if (_IsBuilt)
                return;

            var found = false;
            var materiaSheet = Sheet.Collection.GetSheet<Materia>();
            foreach (var materia in materiaSheet) {
                foreach (var entry in materia.Items) {
                    if (entry.Item == this) {
                        _BaseParam = materia.BaseParam;
                        _Value = entry.Value;
                        found = true;
                        break;
                    }
                }
                if (found)
                    break;
            }

            if (!found) {
                _Value = 0;
                _BaseParam = Sheet.Collection.GetSheet<BaseParam>()[0];
            }

            _IsBuilt = true;
        }
        #endregion
    }
}