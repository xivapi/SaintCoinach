using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    /// Class representing model data (non-humanoid).
    /// </summary>
    public class ModelChara : XivRow {
        #region Properties

        /// <summary>
        /// Gets the type of the current model.
        /// </summary>
        /// <value>The type of the current model.</value>
        /// <remarks>
        /// Confirmed values are:
        ///   2: Demihuman (chara/demihuman/d{Model}/obj/equipment/e{Base}/model/d{Model}e{Base}_{Variant}.mdl)
        ///      -> TODO: Variant to str map
        ///   3: Monster (chara/monster/m{Model}/base/b{Base}/model/m{Model}b{Base}.mdl)
        ///   4: Static object (?)
        ///   5: Attached to other NPC, Golem Soulstone, Titan Heart, etc.
        /// Unconfirmed:
        ///   1: Special body type? Gaius, Nero, Rhitatyn have this
        /// </remarks>
        public int Type { get { return AsInt32("Type"); } }

        /// <summary>
        /// Gets the key for the current model's file.
        /// </summary>
        /// <value>The key for the current model's file.</value>
        public int ModelKey { get { return AsInt32("Model"); } }

        /// <summary>
        /// Gets the key for the base of the model to use.
        /// </summary>
        /// <value>The key for the base of the model to use.</value>
        public int BaseKey { get { return AsInt32("Base"); } }

        /// <summary>
        /// Gets the variant of the model to use.
        /// </summary>
        /// <value>The variant of the model to use.</value>
        public int Variant { get { return AsInt32("Variant"); } }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelChara"/> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet"/> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow"/> to read data from.</param>
        public ModelChara(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
