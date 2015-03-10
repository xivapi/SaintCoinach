using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    using Ex.Relational;

    public class GatheringNotebookList : XivRow {
        #region Fields
        private GatheringItem[] _Items;
        #endregion

        #region Properties

        public IEnumerable<GatheringItem> Items { get { return _Items ?? (_Items = BuildItems()); } }

        #endregion

        #region Constructors

        public GatheringNotebookList(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Build
        private GatheringItem[] BuildItems() {
            const int Count = 100;

            var items = new List<GatheringItem>();

            for (var i = 0; i < Count; ++i) {
                var itm = As<GatheringItem>(i);
                if (itm.Key != 0)
                    items.Add(itm);
            }
            return items.ToArray();
        }
        #endregion
    }
}
