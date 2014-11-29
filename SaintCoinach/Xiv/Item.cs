using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public abstract class Item : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public string Description { get { return AsString("Description"); } }
        public string Singular { get { return AsString("Singular"); } }
        public string Plural { get { return AsString("Plural"); } }
        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }
        public int StackSize { get { return AsInt32("StackSize"); } }
        // TODO: Rarity
        #endregion

        #region Constructor
        protected Item(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}