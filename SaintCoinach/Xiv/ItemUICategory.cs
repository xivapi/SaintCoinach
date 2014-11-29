using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ItemUICategory : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }
        #endregion

        #region Constructor
        public ItemUICategory(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}