namespace SaintCoinach.Xiv {
    public class CraftLeveItem {
        #region Properties

        public Item Item { get; private set; }
        public int Count { get; private set; }
        public bool RequireHq { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public CraftLeveItem(Item item, int count, bool requireHq) {
            Item = item;
            Count = count;
            RequireHq = requireHq;
        }

        #endregion

        #endregion
    }
}
