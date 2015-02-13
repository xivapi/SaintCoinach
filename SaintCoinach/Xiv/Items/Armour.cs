using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.Items {
    /// <summary>
    ///     Class representing armour (left side equipment).
    /// </summary>
    public class Armour : Equipment {
        #region Fields

        /// <summary>
        ///     <see cref="ParameterCollection" /> containing primary parameters of the current item.
        /// </summary>
        private ParameterCollection _PrimaryParameters;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the primary <see cref="Parameter"/>s of the current item.
        /// </summary>
        /// <value>The primary <see cref="Parameter"/>s of the current item.</value>
        public override IEnumerable<Parameter> PrimaryParameters {
            get { return _PrimaryParameters ?? (_PrimaryParameters = BuildPrimaryParameters()); }
        }

        /// <summary>
        ///     Gets the physical defense of the current item.
        /// </summary>
        /// <value>The physical defense of the current item.</value>
        public int PhysicalDefense { get { return AsInt32("Defense{Phys}"); } }

        /// <summary>
        ///     Gets the magical defense of the current item.
        /// </summary>
        /// <value>The magical defense of the current item.</value>
        public int MagicDefense { get { return AsInt32("Defense{Mag}"); } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Armour" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public Armour(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Build

        /// <summary>
        ///     Build the <see cref="ParameterCollection" /> containg the primary parameters of the current item.
        /// </summary>
        /// <returns>The <see cref="ParameterCollection" /> containg the primary parameters of the current item.</returns>
        private ParameterCollection BuildPrimaryParameters() {
            var param = new ParameterCollection();

            // XXX: Here be magic numbers
            const int PhysicalDefenseKey = 21;
            const int MagicDefenseKey = 24;

            var paramSheet = Sheet.Collection.GetSheet<BaseParam>();
            if (PhysicalDefense != 0)
                param.AddParameterValue(paramSheet[PhysicalDefenseKey],
                    new ParameterValueFixed(ParameterType.Primary, PhysicalDefense));
            if (MagicDefense != 0)
                param.AddParameterValue(paramSheet[MagicDefenseKey],
                    new ParameterValueFixed(ParameterType.Primary, MagicDefense));

            return param;
        }

        #endregion
    }
}
