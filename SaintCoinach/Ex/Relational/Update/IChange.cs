namespace SaintCoinach.Ex.Relational.Update {
    public interface IChange {
        #region Properties

        ChangeType ChangeType { get; }
        string SheetName { get; }

        #endregion
    }
}
