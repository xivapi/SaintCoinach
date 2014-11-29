using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class CraftType : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public CraftCrystalType PrimaryCraftCrystal { get { return As<CraftCrystalType>("CraftCrystalType{Primary}"); } }
        public CraftCrystalType SecondaryCraftCrystal { get { return As<CraftCrystalType>("CraftCrystalType{Secondary}"); } }
        #endregion

        #region Constructor
        public CraftType(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}