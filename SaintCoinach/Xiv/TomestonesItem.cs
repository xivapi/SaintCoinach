using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class TomestonesItem : XivRow {
        #region Properties

        public Item Item { get { return As<Item>(); } }
        public int RewardIndex { get { return AsInt32("RewardIndex"); } }

        #endregion

        #region Constructors

        public TomestonesItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
