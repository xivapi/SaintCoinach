using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class Level : XivRow {
        #region Properties

        public float X { get { return AsSingle("X"); } }
        public float Y { get { return AsSingle("Y"); } }
        public float Z { get { return AsSingle("Z"); } }
        public double MapX { get { return ToMapCoordinate(X); } }
        public double MapY { get { return ToMapCoordinate(Z); } }
        public float Yaw { get { return AsSingle("Yaw"); } }
        public float Radius { get { return AsSingle("Radius"); } }
        public int Type { get { return AsInt32("Type"); } }
        public int ObjectKey { get { return AsInt32("ObjectKey"); } }
        public Map Map { get { return As<Map>(); } }

        #endregion

        #region Constructors

        #region Constructor

        public Level(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        private double ToMapCoordinate(double val) {
            var c = Map.Size;

            val *= c;
            return (40.0 / c) * ((val + 1024.0) / 2048.0) + 1;
        }
    }
}
