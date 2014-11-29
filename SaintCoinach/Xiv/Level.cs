using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class Level : XivRow {
        #region Properties
        public float X { get { return AsSingle("X"); } }
        public float Y { get { return AsSingle("Y"); } }
        public float Z { get { return AsSingle("Z"); } }
        public float Yaw { get { return AsSingle("Yaw"); } }
        public float Radius { get { return AsSingle("Radius"); } }
        public int Type { get { return AsInt32("Type"); } }
        public int ObjectKey { get { return AsInt32("ObjectKey"); } }
        public Map Map { get { return As<Map>(); } }
        #endregion

        #region Constructor
        public Level(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

    }
}