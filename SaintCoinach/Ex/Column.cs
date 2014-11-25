using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    public class Column {
        #region Fields
        private Header _Header;
        private int _Index;
        private int _Type;
        private int _Offset;
        private DataReader _Reader;
        #endregion

        #region Properties
        public Header Header { get { return _Header; } }
        public int Index { get { return _Index; } }
        public int Type { get { return _Type; } }
        public int Offset { get { return _Offset; } }
        public DataReader Reader { get { return _Reader; } }
        public virtual string ValueType {
            get { return Reader.Name; }
        }
        #endregion

        #region Constructor
        public Column(Header header, int index, byte[] buffer, int offset) {
            const int TypeOffset = 0x00;
            const int PositionOffset = 0x02;

            _Header = header;
            _Index = index;
            _Type = OrderedBitConverter.ToUInt16(buffer, offset + TypeOffset, true);
            _Offset = OrderedBitConverter.ToUInt16(buffer, offset + PositionOffset, true);

            _Reader = DataReader.GetReader(_Type);
        }
        #endregion

        #region Read
        public virtual object Read(byte[] buffer, IDataRow row) {
            return Reader.Read(buffer, this, row);
        }
        #endregion
    }
}
