using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class BNpcBase : XivRow {
        #region Properties

        /// <summary>
        /// Gets the scale to use for the current BNpc.
        /// </summary>
        /// <value>The scale to use for the current BNpc.</value>
        public double Scale { get { return AsDouble("Scale"); } }

        /// <summary>
        /// Gets the <see cref="ModelChara"/> to use for the current BNpc.
        /// </summary>
        /// <value>The <see cref="ModelChara"/> to use for the current BNpc.</value>
        public ModelChara ModelChara { get { return As<ModelChara>(); } }

        /// <summary>
        /// Gets the BNpcCustomize-row to use for the current BNpc.
        /// </summary>
        /// <value>The BNpcCustomize-row to use for the current BNpc.</value>
        public IXivRow BNpcCustomize { get { return As<IXivRow>("BNpcCustomize"); } }

        /// <summary>
        /// Gets the NpcEquip-row to use for the current BNpc.
        /// </summary>
        /// <value>The NpcEquip-row to use for the current BNpc.</value>
        public IXivRow NpcEquip { get { return As<IXivRow>("NpcEquip"); } }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BNpcBase"/> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet"/> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow"/> to read data from.</param>
        public BNpcBase(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
