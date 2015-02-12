namespace SaintCoinach.Ex {
    public interface IDataSheet<out T> : ISheet<T>, IDataSheet
        where T : IDataRow { }
}
