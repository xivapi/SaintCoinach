using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    [XivSheet("Tomestones")]
    public class Tomestone : XivRow {
        #region Properties

        public Item Item { get { return As<Item>(); } }

        public int WeeklyLimit { get { return AsInt32("WeeklyLimit"); } }

        #endregion

        #region Constructors

        public Tomestone(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
