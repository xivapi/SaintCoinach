using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public abstract class RetainerTaskBase : XivRow, IItemSource {
        #region Properties

        public abstract IEnumerable<Item> Items { get; }

        #endregion

        #region Constructors

        protected RetainerTaskBase(IXivSheet sheet, SaintCoinach.Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
