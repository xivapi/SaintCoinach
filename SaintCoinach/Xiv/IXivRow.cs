using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public interface IXivRow : IRelationalRow {
        #region Properties

        IRelationalRow SourceRow { get; }

        new IXivSheet Sheet { get; }

        #endregion
    }
}
