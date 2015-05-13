using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    using Ex.Relational;

    public class GatheringPointBonusType : XivRow {
        #region Properties

        public Text.XivString Text { get { return AsString("Text"); } }

        #endregion

        #region Constructors

        public GatheringPointBonusType(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Text;
        }
    }
}
