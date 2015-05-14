using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class HousingFurniture : HousingItem {
        public const string ModelPathFormat = "bgcommon/hou/indoor/general/{0:D4}/bgparts/fun_b0_m{0:D4}.mdl";

        #region Constructors

        public HousingFurniture(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override Graphics.ModelDefinition GetModel() {
            var mdlPath = string.Format(ModelPathFormat, ModelKey);

            IO.File baseFile;
            if (!Sheet.Collection.PackCollection.TryGetFile(mdlPath, out baseFile) || !(baseFile is Graphics.ModelFile))
                return null;

            return ((Graphics.ModelFile)baseFile).GetModelDefinition();
        }
    }
}
