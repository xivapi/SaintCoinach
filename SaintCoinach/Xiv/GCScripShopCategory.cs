using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class GCScripShopCategory : XivRow {
        public GrandCompany GrandCompany => As<GrandCompany>();
        public int Tier => AsInt32("Tier");
        public int SubCategory => AsInt32("SubCategory");

        public GCScripShopCategory(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }
    }
}
