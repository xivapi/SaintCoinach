using System;
using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.Items {
    /// <summary>
    ///     Class representing a weapon.
    /// </summary>
    public class Weapon : Equipment {
        #region Fields

        /// <summary>
        ///     <see cref="ParameterCollection" /> containing primary parameters of the current item.
        /// </summary>
        private ParameterCollection _PrimaryParameters;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the primary <see cref="Parameter" />s of the current item.
        /// </summary>
        /// <value>The primary <see cref="Parameter" />s of the current item.</value>
        public override IEnumerable<Parameter> PrimaryParameters {
            get { return _PrimaryParameters ?? (_PrimaryParameters = BuildPrimaryParameters()); }
        }

        /// <summary>
        ///     Gets the physical damage of the current item.
        /// </summary>
        /// <value>The physical damage of the current item.</value>
        public int PhysicalDamage { get { return AsInt32("Damage{Phys}"); } }

        /// <summary>
        ///     Gets the magic damage of the current item.
        /// </summary>
        /// <value>The magic damage of the current item.</value>
        public int MagicDamage { get { return AsInt32("Damage{Mag}"); } }

        /// <summary>
        ///     Gets the delay of the current item.
        /// </summary>
        /// <value>The delay of the current item.</value>
        public TimeSpan Delay { get { return TimeSpan.FromMilliseconds(AsDouble("Delay<ms>")); } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Weapon" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public Weapon(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Build

        /// <summary>
        ///     Build the <see cref="ParameterCollection" /> containg the primary parameters of the current item.
        /// </summary>
        /// <returns>The <see cref="ParameterCollection" /> containg the primary parameters of the current item.</returns>
        private ParameterCollection BuildPrimaryParameters() {
            var param = new ParameterCollection();

            // XXX: Here be magic numbers
            const int PhysicalDamageKey = 12;
            const int MagicDamageKey = 13;
            const int DelayKey = 14;

            var paramSheet = Sheet.Collection.GetSheet<BaseParam>();
            if (PhysicalDamage != 0)
                param.AddParameterValue(paramSheet[PhysicalDamageKey],
                    new ParameterValueFixed(ParameterType.Primary, PhysicalDamage));
            if (MagicDamage != 0)
                param.AddParameterValue(paramSheet[MagicDamageKey],
                    new ParameterValueFixed(ParameterType.Primary, MagicDamage));
            param.AddParameterValue(paramSheet[DelayKey],
                new ParameterValueFixed(ParameterType.Primary, Delay.TotalSeconds));

            return param;
        }

        #endregion
    }
}
