using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    using Ex.Relational;

    public class GatheringPointBase : XivRow, IItemSource {
        #region Fields
        private GatheringItemBase[] _Items;
        #endregion

        #region Properties

        public GatheringType Type { get { return As<GatheringType>(); } }
        public int GatheringLevel { get { return AsInt32("GatheringLevel"); } }
        public IEnumerable<GatheringItemBase> Items { get { return _Items ?? (_Items = BuildItems()); } }
        #endregion

        #region Constructors

        public GatheringPointBase(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Build
        private GatheringItemBase[] BuildItems() {
            const int Count = 8;

            var items = new GatheringItemBase[Count];
            for (var i = 0; i < Count; ++i) {
                var gib = (GatheringItemBase)this["Item[" + i + "]"];
                if (gib?.Key != null && gib?.Item?.Key != null)
                    items[i] = gib;
            }
            return items;
        }
        #endregion

        #region IItemSource Members

        private Item[] _ItemSourceItems;
        IEnumerable<Item> IItemSource.Items {
            get { return _ItemSourceItems ?? (_ItemSourceItems = Items.Where(i => i != null).Select(i => i.Item).OfType<Item>().ToArray()); }
        }

        #endregion
    }
}
