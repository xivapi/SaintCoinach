using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.Items {
    /// <summary>
    /// Class representing a shield.
    /// </summary>
    public class Shield : Equipment {
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
        /// Gets the block strength of the current item.
        /// </summary>
        /// <value>The block strength of the current item.</value>
        public int BlockStrength { get { return AsInt32("Block"); } }
        /// <summary>
        /// Gets the block rate of the current item.
        /// </summary>
        /// <value>The block rate of the current item.</value>
        public int BlockRate { get { return AsInt32("BlockRate"); } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Shield" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public Shield(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Build

        /// <summary>
        ///     Build the <see cref="ParameterCollection" /> containg the primary parameters of the current item.
        /// </summary>
        /// <returns>The <see cref="ParameterCollection" /> containg the primary parameters of the current item.</returns>
        private ParameterCollection BuildPrimaryParameters() {
            var param = new ParameterCollection();

            // XXX: Here be magic numbers
            const int BlockStrengthKey = 17;
            const int BlockRateKey = 18;

            var paramSheet = Sheet.Collection.GetSheet<BaseParam>();
            if (BlockStrength != 0)
                param.AddParameterValue(paramSheet[BlockStrengthKey],
                    new ParameterValueFixed(ParameterType.Primary, BlockStrength));
            if (BlockRate != 0)
                param.AddParameterValue(paramSheet[BlockRateKey],
                    new ParameterValueFixed(ParameterType.Primary, BlockRate));

            return param;
        }

        #endregion
    }
}
