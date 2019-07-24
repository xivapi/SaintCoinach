using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class CraftLevelDifference : XivRow {
        public int Difference => AsInt32("Difference");
        public int ProgressFactor => AsInt32("ProgressFactor");
        public int QualityFactor => AsInt32("QualityFactor");

        public CraftLevelDifference(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }
    }
}
