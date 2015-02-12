using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.Items {
    public class SoulCrystal : Equipment {
        #region Properties

        public override IEnumerable<Parameter> PrimaryParameters { get { yield break; } }

        #endregion

        #region Constructors

        #region Constructor

        public SoulCrystal(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
