using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class Stain : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public Item Item { get { return As<Item>("Item"); } }
        public int Shade { get { return AsInt32("Shader"); } }
        public System.Drawing.Color Color { get { return As<System.Drawing.Color>(); } }
        #endregion

        #region Constructor
        public Stain(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}