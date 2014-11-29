using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class Map : XivRow {
        #region Properties
        public string Id { get { return AsString("Id"); } }
        public int Size { get { return AsInt32("Size"); } }
        public PlaceName RegionPlaceName { get { return As<PlaceName>("PlaceName{Region}"); } }
        public PlaceName PlaceName { get { return As<PlaceName>(); } }
        public PlaceName LocationPlaceName { get { return As<PlaceName>("PlaceName{Sub}"); } }
        public TerritoryType TerritoryType { get { return As<TerritoryType>(); } }
        #endregion

        #region Constructor
        public Map(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Id;
        }
    }
}