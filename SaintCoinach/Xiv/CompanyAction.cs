using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class CompanyAction : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public string Description { get { return AsString("Description"); } }
        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }
        public FCRank FCRank { get { return As<FCRank>(); } }
        public int Cost { get { return AsInt32("Cost"); } }
        public int Order { get { return AsInt32("Order"); } }
        #endregion

        #region Constructor
        public CompanyAction(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}