using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class Title : XivRow {
        #region Properties
        public string Masculine { get { return AsString("Masculine"); } }
        public string Feminine { get { return AsString("Feminine"); } }
        public bool IsPrefix { get { return AsBoolean("IsPrefix"); } }
        #endregion

        #region Constructor
        public Title(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return string.Format("{0} / {1}", Feminine, Masculine);
        }
    }
}