using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class Map : ComponentContainer {
        #region Fields
        private string _BasePath;
        private IO.PackCollection _PackCollection;
        private Xiv.TerritoryType _TerritoryType;
        #endregion

        #region Properties
        public string BasePath { get { return _BasePath; } }
        public Xiv.TerritoryType TerritoryType { get { return _TerritoryType; } }
        #endregion

        #region Constructor
        public Map(IO.PackCollection packCollection, Xiv.TerritoryType territoryType) {
            _PackCollection = packCollection;

            var bg = territoryType.Bg;
            var i = bg.IndexOf("/level/");
            if (i < 0)
                throw new NotSupportedException();
            _BasePath = bg.Substring(0, i);
            _TerritoryType = territoryType;

            PreProcess();
        }
        public Map(IO.PackCollection packCollection, string basePath) {
            _PackCollection = packCollection;
            _BasePath = basePath;

            PreProcess();
        }
        #endregion

        #region Pre
        private void PreProcess() {
            IO.Directory terrainDir;
            if (_PackCollection.TryGetDirectory(_BasePath + "/bgplate", out terrainDir))
                Add(new Parts.BgPlate(terrainDir));
        }
        #endregion
    }
}
