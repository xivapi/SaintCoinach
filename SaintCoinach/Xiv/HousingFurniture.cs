using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class HousingFurniture : HousingItem {
        public const string SgbPathFormat = "bgcommon/hou/indoor/general/{0:D4}/asset/fun_b0_m{0:D4}.sgb";

        #region Constructors

        public HousingFurniture(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

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
