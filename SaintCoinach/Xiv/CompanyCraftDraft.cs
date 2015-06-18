using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class CompanyCraftDraft : XivRow {
        #region Fields

        private RequiredItem[] _RequiredItems;

        #endregion

        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }

        public CompanyCraftDraftCategory CompanyCraftDraftCategory { get { return As<CompanyCraftDraftCategory>(); } }

        public IEnumerable<RequiredItem> RequiredItems { get { return _RequiredItems ?? (_RequiredItems = BuildRequiredItems()); } }

        public int Order { get { return AsInt32("Order"); } }

        #endregion

        #region Constructors

        public CompanyCraftDraft(IXivSheet sheet, SaintCoinach.Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        RequiredItem[] BuildRequiredItems() {
            const int Count = 3;

            var items = new List<RequiredItem>();
            for (var i = 0; i < Count; ++i) {
                var item = As<Item>("RequiredItem", i);
                var count = AsInt32("RequiredItemCount", i);
                if (item == null || item.Key == 0 || count == 0)
                    continue;
                items.Add(new RequiredItem(item, count));
            }
            return items.ToArray();
        }

        public override string ToString() {
            return Name;
        }

        public class RequiredItem {
            public Item Item { get; private set; }
            public int Count { get; private set; }

            internal RequiredItem(Item item, int count) {
                Item = item;
                Count = count;
            }
        }
    }
}