using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    using Ex.Relational;

    public class GatheringPointBase : XivRow, IItemSource {
        #region Fields
        private GatheringItem[] _GatheringItems;
        #endregion

        #region Properties

        public GatheringType Type { get { return As<GatheringType>(); } }
        public int Level { get { return AsInt32("Level"); } }
        public IEnumerable<GatheringItem> GatheringItems { get { return _GatheringItems ?? (_GatheringItems = BuildGatheringItems()); } }
        public bool IsLimited { get { return AsBoolean("IsLimited"); } }
        #endregion

        #region Constructors

        public GatheringPointBase(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Build
        private GatheringItem[] BuildGatheringItems() {
            const int Count = 8;

            var items = new GatheringItem[Count];
            for (var i = 0; i < Count; ++i) {
                var gi = As<GatheringItem>(i);
                if (gi.Key != 0 && gi.Item != null && gi.Item.Key != 0)
                    items[i] = gi;
            }
            return items;
        }
        #endregion

        #region IItemSource Members

        private Item[] _ItemSourceItems;
        IEnumerable<Item> IItemSource.Items {
            get { return _ItemSourceItems ?? (_ItemSourceItems = GatheringItems.Where(i => i != null).Select(i => i.Item).ToArray()); }
        }

        #endregion
    }
}
