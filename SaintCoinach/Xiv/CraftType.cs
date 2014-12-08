using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class CraftType : XivRow {
        // XXX: Here be magic number
        const int ClassJobOffset = 8;

        #region Properties
        public string Name { get { return AsString("Name"); } }
        public CraftCrystalType PrimaryCraftCrystal { get { return As<CraftCrystalType>("CraftCrystalType{Primary}"); } }
        public CraftCrystalType SecondaryCraftCrystal { get { return As<CraftCrystalType>("CraftCrystalType{Secondary}"); } }
        public ClassJob ClassJob { get { return Sheet.Collection.GetSheet<ClassJob>()[ClassJobOffset + this.Key]; } }
        #endregion

        #region Constructor
        public CraftType(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}