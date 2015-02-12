using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.Items {
    public class MateriaItem : Item {
        #region Fields

        private BaseParam _BaseParam;
        private bool _IsBuilt;
        private int _Tier;
        private int _Value;

        #endregion

        #region Properties

        public BaseParam BaseParam {
            get {
                Build();
                return _BaseParam;
            }
        }

        public int Value {
            get {
                Build();
                return _Value;
            }
        }

        public int Tier {
            get {
                Build();
                return _Tier;
            }
        }

        #endregion

        #region Constructors

        #region Constructor

        public MateriaItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Build

        private void Build() {
            if (_IsBuilt)
                return;

            var found = false;
            var materiaSheet = Sheet.Collection.GetSheet<Materia>();
            foreach (var materia in materiaSheet) {
                var entry = materia.Items.FirstOrDefault(e => e.Item == this);
                if (entry == null) continue;

                _BaseParam = materia.BaseParam;
                _Value = entry.Value;
                _Tier = entry.Tier;
                found = true;
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
