using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class LeveRewardItemGroup : XivRow {
        #region Fields

        private Entry[] _Items;

        #endregion

        #region Properties

        public IEnumerable<Entry> Items { get { return _Items ?? (_Items = BuildItems()); } }

        #endregion

        #region Constructors

        #region Constructor

        public LeveRewardItemGroup(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Build

        private Entry[] BuildItems() {
            const int Count = 9;

            var items = new List<Entry>();
            for (var i = 0; i < Count; ++i) {
                var count = AsInt32("Count", i);
                if (count == 0)
                    continue;

                var item = As<Item>("Item", i);
                if (item.Key == 0)
                    continue;

                var hq = AsBoolean("HQ", i);

                items.Add(new Entry(item, count, hq));
            }

            return items.ToArray();
        }

        #endregion

        #region Helper class

        public class Entry {
            #region Properties

            public Item Item { get; private set; }
            public int Count { get; private set; }
            public bool IsHq { get; private set; }

            #endregion

            #region Constructors

            #region Constructor

            public Entry(Item item, int count, bool isHq) {
                Item = item;
                Count = count;
                IsHq = isHq;
            }

            #endregion

            #endregion
        }

        #endregion
    }
}
