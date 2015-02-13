using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing materia groups.
    /// </summary>
    public class Materia : XivRow {
        #region Fields

        /// <summary>
        ///     Item and value pairs for the current materia group.
        /// </summary>
        private ItemValue[] _Items;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the <see cref="BaseParam" /> granted by the current materia group.
        /// </summary>
        /// <value>The <see cref="BaseParam" /> granted by the current materia group.</value>
        public BaseParam BaseParam { get { return As<BaseParam>(); } }

        /// <summary>
        ///     Gets the item and value pairs of the current materia group.
        /// </summary>
        /// <value>The item and value pairs of the current materia group.</value>
        public IEnumerable<ItemValue> Items { get { return _Items ?? (_Items = BuildItems()); } }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Materia" /> class.
        /// </summary>
        /// <param name="sheet"><see cref="IXivSheet" /> containing this object.</param>
        /// <param name="sourceRow"><see cref="IRelationalRow" /> to read data from.</param>
        public Materia(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #region Build

        /// <summary>
        ///     Build an array of item and value pairs of the current materia group.
        /// </summary>
        /// <returns>An array of item and value pairs of the current materia group.</returns>
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

        #region Item value helper class

        /// <summary>
        ///     Class representing a item and value pair for a materia group.
        /// </summary>
        public class ItemValue {
            #region Properties

            /// <summary>
            ///     Gets the <see cref="Item" /> of the current pair.
            /// </summary>
            /// <value>The <see cref="Item" /> of the current pair.</value>
            public Item Item { get; private set; }

            /// <summary>
            ///     Gets the value of the current pair.
            /// </summary>
            /// <value>The value of the current pair.</value>
            public int Value { get; private set; }

            /// <summary>
            ///     Gets the materia tier of the current pair.
            /// </summary>
            /// <value>The materia tier of the current pair.</value>
            public int Tier { get; private set; }

            #endregion

            #region Constructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="ItemValue" /> class.
            /// </summary>
            /// <param name="item"><see cref="Item" /> of the pair.</param>
            /// <param name="value">Value of the pair.</param>
            /// <param name="tier">Tier of the pair.</param>
            public ItemValue(Item item, int value, int tier) {
                Item = item;
                Value = value;
                Tier = tier;
            }

            #endregion
        }

        #endregion
    }
}
