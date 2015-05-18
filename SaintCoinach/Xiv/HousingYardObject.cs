using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class HousingYardObject : HousingItem {
        public const string SgbPathFormat = "bgcommon/hou/outdoor/general/{0:D4}/asset/gar_b0_m{0:D4}.sgb";

        #region Constructors

        public HousingYardObject(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override Graphics.Sgb.SgbFile GetScene() {
            var sgbPath = string.Format(SgbPathFormat, ModelKey);

            IO.File baseFile;
            if (!Sheet.Collection.PackCollection.TryGetFile(sgbPath, out baseFile))
                return null;

            return new Graphics.Sgb.SgbFile(baseFile);
        }
    }
}
