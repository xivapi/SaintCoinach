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
        private int _Tier;
        #endregion

        #region Properties
        public BaseParam BaseParam { get { Build(); return _BaseParam; } }
        public int Value { get { Build(); return _Value; } }
        public int Tier { get { Build(); return _Tier; } }
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
                        _Tier = entry.Tier;
                        found = true;
                        break;
                    }
                }
                if (found)
                    break;
            }

            if (!found) {
                _Tier = -1;
                _Value = 0;
                _BaseParam = Sheet.Collection.GetSheet<BaseParam>()[0];
            }

            _IsBuilt = true;
        }
        #endregion
    }
}