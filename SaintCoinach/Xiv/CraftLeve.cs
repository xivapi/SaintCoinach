using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class CraftLeve : XivRow {
        #region Fields

        private CraftLeveItem[] _Items;

        #endregion

        #region Properties

        public Leve Leve { get { return As<Leve>(); } }
        public int Repeats { get { return AsInt32("Repeats"); } }
        public IEnumerable<CraftLeveItem> Items { get { return _Items ?? (_Items = BuildItems()); } }

        #endregion

        #region Constructors

        #region Constructor

        public CraftLeve(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Build

        private CraftLeveItem[] BuildItems() {
            const int Count = 4;

            var items = new List<CraftLeveItem>();

            for (var i = 0; i < Count; ++i) {
                var count = AsInt32("ItemCount", i);
                if (count == 0)
                    continue;

                var item = As<Item>("Item", i);
                if (item.Key == 0)
                    continue;

                var hq = AsBoolean("HQ", i);

                items.Add(new CraftLeveItem(item, count, hq));
            }

            return items.ToArray();
        }

        #endregion
    }
}
