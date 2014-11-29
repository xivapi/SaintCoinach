using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class BuddyEquip : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public string Singular { get { return AsString("Singular"); } }
        public string Plural { get { return AsString("Plural"); } }
        public GrandCompany GrandCompany { get { return As<GrandCompany>(); } }
        public Imaging.ImageFile HeadIcon { get { return AsImage("Icon{Head}"); } }
        public Imaging.ImageFile BodyIcon { get { return AsImage("Icon{Body}"); } }
        public Imaging.ImageFile LegsIcon { get { return AsImage("Icon{Legs}"); } }
        #endregion

        #region Constructor
        public BuddyEquip(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}