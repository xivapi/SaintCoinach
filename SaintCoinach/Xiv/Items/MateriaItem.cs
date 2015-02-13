using System.Linq;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.Items {
    /// <summary>
    ///     Class representing a materia item.
    /// </summary>
    public class MateriaItem : Item {
        #region Fields

        /// <summary>
        ///     <see cref="BaseParam" /> granted by the current materia item.
        /// </summary>
        private BaseParam _BaseParam;

        /// <summary>
        ///     Value indicating whether <see cref="BaseParam" />, <see cref="Tier" /> and <see cref="Value" /> have been
        ///     retrieved.
        /// </summary>
        private bool _IsBuilt;

        /// <summary>
        ///     Tier of the current materia item.
        /// </summary>
        private int _Tier;

        /// <summary>
        ///     Bonus granted to <see cref="BaseParam" /> by the current materia item.
        /// </summary>
        private int _Value;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the <see cref="BaseParam" /> granted by the current materia item.
        /// </summary>
        /// <value>The <see cref="BaseParam" /> granted by the current materia item.</value>
        public BaseParam BaseParam {
            get {
                Build();
                return _BaseParam;
            }
        }

        /// <summary>
        ///     Gets the bonus granted to <see cref="BaseParam" /> by the current materia item.
        /// </summary>
        /// <value>The bonus granted to <see cref="BaseParam" /> by the current materia item.</value>
        public int Value {
            get {
                Build();
                return _Value;
            }
        }

        /// <summary>
        ///     Gets the tier of the current materia item.
        /// </summary>
        /// <value>The tier of the current materia item.</value>
        public int Tier {
            get {
                Build();
                return _Tier;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MateriaItem" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public MateriaItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Build

        /// <summary>
        ///     Retrieve the <see cref="BaseParam" />, <see cref="Value" /> and <see cref="Tier" /> from the <see cref="Materia" />
        ///     to which the current item is assigned.
        /// </summary>
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
