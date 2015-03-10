using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class RacingChocoboItem : XivRow {
        #region Properties

        public Item Item { get { return As<Item>(); } }
        public int Category { get { return AsInt32("Category"); } }
        public int Param1 { get { return AsInt32("Param", 0); } }
        public int Param2 { get { return AsInt32("Param", 1); } }

        #endregion

        #region Constructors

        public RacingChocoboItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
