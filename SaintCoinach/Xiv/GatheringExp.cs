using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    using Ex.Relational;

    public class GatheringExp : XivRow {
        #region Properties

        public int Exp { get { return AsInt32("Exp"); } }

        #endregion

        #region Constructors

        public GatheringExp(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
