using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class FishParameter : XivRow {
        #region Properties
        public string Text { get { return AsString("Text"); } }
        public Item Item { get { return As<Item>("Item"); } }
        #endregion

        #region Constructor
        public FishParameter(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return string.Format("{0}", Item);
        }
    }
}