using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class GrandCompany : XivRow {
        const int SealItemOffset = 19;

        #region Properties
        public string Name { get { return AsString("Name"); } }
        public Item SealItem { get { return Sheet.Collection.GetSheet<Item>()[Key + SealItemOffset]; } }
        #endregion

        #region Constructor
        public GrandCompany(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}