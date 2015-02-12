using System.Drawing;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class Stain : XivRow {
        #region Properties

        public string Name { get { return AsString("Name"); } }
        public Item Item { get { return As<Item>("Item"); } }
        public int Shade { get { return AsInt32("Shader"); } }
        public Color Color { get { return As<Color>(); } }

        #endregion

        #region Constructors

        #region Constructor

        public Stain(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
