using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    using Ex.Relational;

    public class GatheringNotebookRegion : XivRow {
        #region Properties

        public PlaceName PlaceName { get { return As<PlaceName>(); } }

        #endregion

        #region Constructors

        public GatheringNotebookRegion(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return PlaceName.Name;
        }
    }
}
