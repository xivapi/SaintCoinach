using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class Materia : XivRow {
        #region Fields

        #region FIelds

        private ItemValue[] _Items;

        #endregion

        #endregion

        #region Properties

        public BaseParam BaseParam { get { return As<BaseParam>(); } }
        public IEnumerable<ItemValue> Items { get { return _Items ?? (_Items = BuildItems()); } }

        #endregion

        #region Constructors

        #region Constructor

        public Materia(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Build

        private ItemValue[] BuildItems() {
            const int Count = 10;

            var items = new List<ItemValue>();

            for (var i = 0; i < Count; ++i) {
                var item = As<Item>("Item", i);
                if (item.Key == 0)
                    continue;

                var val = AsInt32("Value", i);
                items.Add(new ItemValue(item, val, i));
            }

            return items.ToArray();
        }

        #endregion

        #region MateriaItem

        public class ItemValue {
            #region Properties

            public Item Item { get; private set; }
            public int Value { get; private set; }
            public int Tier { get; private set; }

            #endregion

            #region Constructors

            #region Constructor

            public ItemValue(Item item, int value, int tier) {
                Item = item;
                Value = value;
                Tier = tier;
            }

            #endregion

            #endregion
        }

        #endregion
    }
}
