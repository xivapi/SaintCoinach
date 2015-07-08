using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class RetainerTaskNormal : RetainerTaskBase {
        #region Properties

        public Item Item { get { return As<Item>(); } }
        public IEnumerable<int> Quantities {
            get {
                const int Count = 3;
                for (var i = 0; i < Count; ++i)
                    yield return AsInt32("Quantity", i);
            }
        }
        public override IEnumerable<Item> Items { get { yield return Item; } }

        #endregion

        #region Constructors

        public RetainerTaskNormal(IXivSheet sheet, SaintCoinach.Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
