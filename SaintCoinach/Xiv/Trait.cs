using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class Trait : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public string Description { get { return AsString("Description"); } }
        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }
        public ClassJob ClassJob { get { return As<ClassJob>(); } }
        public int Level { get { return AsInt32("Level"); } }
        public ClassJobCategory ClassJobCategory { get { return As<ClassJobCategory>(); } }
        public int Value { get { return AsInt32("Value"); } }
        #endregion

        #region Constructor
        public Trait(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}