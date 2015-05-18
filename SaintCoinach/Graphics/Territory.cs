using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class Territory {
        #region Properties
        public IO.PackCollection Packs { get; private set; }
        public TerritoryParts.Terrain Terrain { get; private set; }
        public string BasePath { get; private set; }
        public string Name { get; private set; }
        public Lgb.LgbFile[] LgbFiles { get; private set; }
        #endregion

        #region Constructor
        public Territory(Xiv.TerritoryType type) : this(type.Sheet.Collection.PackCollection, type.Name, type.Bg) { }

        /// <param name="levelPath">Not including bg/</param>
        public Territory(IO.PackCollection packs, string name, string levelPath) {
            this.Packs = packs;
            this.Name = name;
            var i = levelPath.IndexOf("/level/");
            this.BasePath = "bg/" + levelPath.Substring(0, i + 1);

            Build();
        }
        #endregion

        #region Build
        private void Build() {
            IO.File terrainFile;
            var terrainPath = BasePath + "bgplate/terrain.tera";
            if (Packs.TryGetFile(terrainPath, out terrainFile))
                this.Terrain = new TerritoryParts.Terrain(terrainFile);

            var lgbFiles = new List<Lgb.LgbFile>();
            lgbFiles.Add(TryGetLgb("level/bg.lgb"));

            this.LgbFiles = lgbFiles.Where(l => l != null).ToArray();
        }
        private Lgb.LgbFile TryGetLgb(string name) {
            var path = BasePath + name;
            IO.File file;
            if (Packs.TryGetFile(path, out file))
                return new Lgb.LgbFile(file);
            return null;
        }
        #endregion
    }
}
