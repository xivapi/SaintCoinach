using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class TomestonesItem : XivRow {
        #region Properties

        public Item Item => As<Item>();
        public Tomestone Tomestone => As<Tomestone>("Tomestones");

        #endregion

        #region Constructors

        public TomestonesItem(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
