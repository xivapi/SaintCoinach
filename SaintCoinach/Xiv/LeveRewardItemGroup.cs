using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class LeveRewardItemGroup : XivRow {
        #region Helper class
        public class Entry {
            #region Fields
            private Item _Item;
            private int _Count;
            private bool _IsHq;
            #endregion

            #region Properties
            public Item Item { get { return _Item; } }
            public int Count { get { return _Count; } }
            public bool IsHq { get { return _IsHq; } }
            #endregion

            #region Constructor
            public Entry(Item item, int count, bool isHq) {
                _Item = item;
                _Count = count;
                _IsHq = isHq;
            }
            #endregion
        }
        #endregion

        #region Fields
        private Entry[] _Items;
        #endregion

        #region Properties
        public IEnumerable<Entry> Items { get { return _Items ?? (_Items = BuildItems()); } }
        #endregion

        #region Constructor
        public LeveRewardItemGroup(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
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
    }
}