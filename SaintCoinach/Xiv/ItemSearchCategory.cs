using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ItemSearchCategory : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }
        public int Category { get { return AsInt32("Category"); } }
        public int Order { get { return AsInt32("Order"); } }
        public ClassJob ClassJob { get { return As<ClassJob>(); } }
        #endregion

        #region Constructor
        public ItemSearchCategory(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}