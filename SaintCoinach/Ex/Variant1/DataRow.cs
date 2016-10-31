using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex.Variant1 {
    public class DataRow : DataRowBase {
        const int MetadataLength = 0x06;

        public int Length { get; private set; }

        #region Constructors

        public DataRow(IDataSheet sheet, int key, int offset) : base(sheet, key, offset + MetadataLength) {
            var b = sheet.GetBuffer();
            if (b.Length < offset + MetadataLength) throw new IndexOutOfRangeException();

            Length = OrderedBitConverter.ToInt32(b, offset, true);
            var c = OrderedBitConverter.ToInt16(b, offset + 4, true);
            if (c != 1) throw new InvalidDataException();
        }

        #endregion
    }
}
