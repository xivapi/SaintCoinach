using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class ENpcBase : XivRow {
        #region Static

        public const int DataCount = 32;

        #endregion

        #region Constructors

        #region Constructor

        public ENpcBase(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public int GetData(int index) {
            return AsInt32("ENpcData", index);
        }
    }
}
