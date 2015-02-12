using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.Items {
    public class CraftingTool : Equipment {
        #region Properties

        public override IEnumerable<Parameter> PrimaryParameters { get { yield break; } }

        #endregion

        #region Constructors

        #region Constructor

        public CraftingTool(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
