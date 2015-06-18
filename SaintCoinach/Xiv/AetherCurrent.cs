using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class AetherCurrent : XivRow {
        #region Properties

        public Quest Quest { get { return As<Quest>(); } }

        #endregion

        #region Constructors

        public AetherCurrent(IXivSheet sheet, SaintCoinach.Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
