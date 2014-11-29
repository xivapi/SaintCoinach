using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class BuddyAction : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public string Description { get { return AsString("Description"); } }
        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }
        public Imaging.ImageFile StatusIcon { get { return AsImage("Icon{Status}"); } }
        #endregion

        #region Constructor
        public BuddyAction(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}