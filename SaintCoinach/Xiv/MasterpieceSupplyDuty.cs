using SaintCoinach.Ex.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public partial class MasterpieceSupplyDuty : XivRow {
        #region Fields
        private CollectableItem[] _CollectableItems;
        #endregion
        
        #region Properties
        public ClassJob ClassJob { get { return As<ClassJob>(); } }
        public int ItemLevel { get { return AsInt32("ItemLevel"); } }
        public Item RewardItem { get { return As<Item>("RewardItem"); } }
        public IEnumerable<CollectableItem> CollectableItems { get { return _CollectableItems ?? (_CollectableItems = BuildCollectableItems()); } }
        #endregion

        #region Constructors
        public MasterpieceSupplyDuty(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        private CollectableItem[] BuildCollectableItems() {
            const int Count = 8;

            var items = new CollectableItem[Count];
            for (var i = 0; i < Count; ++i)
                items[i] = new CollectableItem(this, i);
            return items;
        }
        #endregion
    }
}
