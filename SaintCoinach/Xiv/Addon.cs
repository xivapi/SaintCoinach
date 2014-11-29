using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class Addon : XivRow {
        #region Properties
        public string Text { get { return AsString("Text"); } }
        #endregion

        #region Constructor
        public Addon(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Text;
        }
    }
}