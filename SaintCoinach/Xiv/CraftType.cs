using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class CraftType : XivRow {
        #region Static

        // XXX: Here be magic number
        private const int ClassJobOffset = 8;

        #endregion

        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }

        public CraftCrystalType PrimaryCraftCrystal {
            get { return As<CraftCrystalType>("CraftCrystalType{Primary}"); }
        }

        public CraftCrystalType SecondaryCraftCrystal {
            get { return As<CraftCrystalType>("CraftCrystalType{Secondary}"); }
        }

        public ClassJob ClassJob { get { return Sheet.Collection.GetSheet<ClassJob>()[ClassJobOffset + Key]; } }

        #endregion

        #region Constructors

        #region Constructor

        public CraftType(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
