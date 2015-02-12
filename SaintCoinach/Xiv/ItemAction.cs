using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class ItemAction : XivRow {
        #region Static

        public const int DataCount = 8;

        #endregion

        #region Properties

        public int Type { get { return AsInt32("Type"); } }

        #endregion

        #region Constructors

        #region Constructor

        public ItemAction(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Helpers

        public int GetData(int index) {
            return AsInt32("Data", index);
        }

        public int GetHqData(int index) {
            return AsInt32("Data{HQ}", index);
        }

        #endregion
    }
}
