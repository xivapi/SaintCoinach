using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ENpcBase : XivRow {
        public const int DataCount = 32;

        #region Properties
        public int GetData(int index) {
            return AsInt32("ENpcData", index);
        }
        #endregion

        #region Constructor
        public ENpcBase(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion
    }
}