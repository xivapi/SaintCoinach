namespace SaintCoinach.Ex {
    public class Column {
        #region Properties

        public Header Header { get; private set; }
        public int Index { get; private set; }
        public int Type { get; private set; }
        public int Offset { get; private set; }
        public DataReader Reader { get; private set; }
        public virtual string ValueType { get { return Reader.Name; } }

        #endregion

        #region Constructors

        #region Constructor

        public Column(Header header, int index, byte[] buffer, int offset) {
            const int TypeOffset = 0x00;
            const int PositionOffset = 0x02;

            Header = header;
            Index = index;
            Type = OrderedBitConverter.ToUInt16(buffer, offset + TypeOffset, true);
            Offset = OrderedBitConverter.ToUInt16(buffer, offset + PositionOffset, true);

            Reader = DataReader.GetReader(Type);
        }

        #endregion

        #endregion

        #region Read

        public virtual object Read(byte[] buffer, IDataRow row) {
            return Reader.Read(buffer, this, row);
        }

        #endregion
    }
}
