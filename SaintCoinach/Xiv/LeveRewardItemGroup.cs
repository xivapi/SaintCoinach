using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing a group of items that can appear as rewards for leves.
    /// </summary>
    /// <remarks>
    ///     This is not confirmed, but it seems like all entries in a group have the same chance of appearing if the group is
    ///     picked.
    /// </remarks>
    public class LeveRewardItemGroup : XivRow {
        #region Fields

        /// <summary>
        ///     The entries in the current group.
        /// </summary>
        private Entry[] _Items;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the entries in the current group.
        /// </summary>
        /// <value>The entries in the current group.</value>
        public IEnumerable<Entry> Items { get { return _Items ?? (_Items = BuildItems()); } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LeveRewardItemGroup" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public LeveRewardItemGroup(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Build

        /// <summary>
        ///     Build an array of entries in the current <see cref="LeveRewardItemGroup" />.
        /// </summary>
        /// <returns>An array of entries in the current <see cref="LeveRewardItemGroup" />.</returns>
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

        /// <summary>
        ///     Class representing an entry in a <see cref="LeveRewardItemGroup" />.
        /// </summary>
        public class Entry {
            #region Properties

            /// <summary>
            ///     Gets the item for the current entry.
            /// </summary>
            /// <value>The item for the current entry.</value>
            public Item Item { get; private set; }

            /// <summary>
            ///     Gets the item count for the current entry.
            /// </summary>
            /// <value>The item count for the current entry.</value>
            public int Count { get; private set; }

            /// <summary>
            ///     Gets a value indicating whether the <see cref="Item" /> is high-quality.
            /// </summary>
            /// <value>A value indicating whether the <see cref="Item" /> is high-quality.</value>
            public bool IsHq { get; private set; }

            #endregion

            #region Constructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="Entry" /> class.
            /// </summary>
            /// <param name="item"><see cref="Item" /> for the entry.</param>
            /// <param name="count">Item count of the entry.</param>
            /// <param name="isHq">A value indicating whether the <c>item</c> is high-quality.</param>
            public Entry(Item item, int count, bool isHq) {
                Item = item;
                Count = count;
                IsHq = isHq;
            }

            #endregion
        }

        #endregion
    }
}
