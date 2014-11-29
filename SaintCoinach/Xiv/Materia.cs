using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class Materia : XivRow {
        #region MateriaItem
        public class ItemValue {
            #region Fields
            private Item _Item;
            private int _Value;
            #endregion

            #region Properties
            public Item Item { get { return _Item; } }
            public int Value { get { return _Value; } }
            #endregion

            #region Constructor
            public ItemValue(Item item, int value) {
                _Item = item;
                _Value = value;
            }
            #endregion
        }
        #endregion

        #region FIelds
        private ItemValue[] _Items = null;
        #endregion

        #region Properties
        public BaseParam BaseParam { get { return As<BaseParam>(); } }
        public IEnumerable<ItemValue> Items { get { return _Items ?? (_Items = BuildItems()); } }
        #endregion

        #region Constructor
        public Materia(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
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
                items.Add(new ItemValue(item, val));
            }

            return items.ToArray();
        }
        #endregion
    }
}