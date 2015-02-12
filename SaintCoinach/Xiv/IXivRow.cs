using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public interface IXivRow : IRelationalRow {
        #region Properties

        new IXivSheet Sheet { get; }

        #endregion
    }
}
