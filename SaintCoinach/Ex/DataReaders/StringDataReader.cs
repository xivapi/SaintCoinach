using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.DataReaders {
    public class StringDataReader : DataReader {
        #region Properties
        public override int Length {
            get { return 4; }
        }
        public override string Name {
            get { return "str"; }
        }
        public override Type Type {
            get { return typeof(string); }
        }
        #endregion

        #region Convert
        public override object Read(byte[] data, Column col, IDataRow row) {
            var fieldOffset = GetFieldOffset(col, row);
            var endOfFixed = row.Offset + row.Sheet.Header.FixedSizeDataLength;

            var start = endOfFixed + OrderedBitConverter.ToInt32(data, fieldOffset, true);
            if (start < 0)
                return null;

            var end = start - 1;
            while (++end < data.Length && data[end] != 0) ;
            var len = end - start;

            if (len == 0)
                return string.Empty;

            var binaryString = new byte[len];
            Array.Copy(data, start, binaryString, 0, len);

            return StringHelper.Decode(binaryString);
        }
        #endregion
    }
}
