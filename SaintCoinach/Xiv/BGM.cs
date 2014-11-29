using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class BGM : XivRow {
        #region Properties
        public string File { get { return AsString("File"); } }
        #endregion

        #region Constructor
        public BGM(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return File;
        }
    }
}