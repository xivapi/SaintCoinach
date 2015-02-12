namespace SaintCoinach.Ex {
    public interface IDataSheet : ISheet {
        #region Properties

        Language Language { get; }

        #endregion

        byte[] GetBuffer();
    }
}
