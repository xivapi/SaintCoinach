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
            this.BasePath = ResolveBasePath(levelPath);

            Build();
        }
        #endregion

        #region Build
        private string ResolveBasePath(string levelPath) {
            var normalized = (levelPath ?? string.Empty).Replace('\\', '/').Trim().Trim('/');
            if (normalized.StartsWith("bg/", StringComparison.OrdinalIgnoreCase))
                normalized = normalized.Substring(3);

            var candidates = new List<string>();

            var levelSegmentIndex = normalized.IndexOf("/level/", StringComparison.OrdinalIgnoreCase);
            if (levelSegmentIndex >= 0)
                candidates.Add("bg/" + normalized.Substring(0, levelSegmentIndex + 1));

            if (!string.IsNullOrEmpty(normalized))
                candidates.Add("bg/" + normalized.TrimEnd('/') + "/");

            if (normalized.EndsWith("/level", StringComparison.OrdinalIgnoreCase)) {
                var withoutLevel = normalized.Substring(0, normalized.Length - "/level".Length).TrimEnd('/');
                if (!string.IsNullOrEmpty(withoutLevel))
                    candidates.Add("bg/" + withoutLevel + "/");
            }

            candidates.Add("bg/");

            foreach (var candidate in candidates.Distinct(StringComparer.OrdinalIgnoreCase)) {
                if (LooksLikeValidBasePath(candidate)) {
                    return candidate;
                } 
            }

            var fallback = candidates[0];
            System.Diagnostics.Debug.WriteLine(
                string.Format("Could not verify territory base path candidates. Bg='{0}', Fallback='{1}'", levelPath, fallback));
            return fallback;
        }

        private bool LooksLikeValidBasePath(string basePath) {
            return Packs.FileExists(basePath + "bgplate/terrain.tera")
                || Packs.FileExists(basePath + "level/bg.lgb")
                || Packs.FileExists(basePath + "level/planmap.lgb")
                || Packs.FileExists(basePath + "level/planevent.lgb");
        }

        private void Build() {
            var terrainPath = BasePath + "bgplate/terrain.tera";
            if (Packs.TryGetFile(terrainPath, out var terrainFile))
                this.Terrain = new TerritoryParts.Terrain(terrainFile);

            var lgbFiles = new List<Lgb.LgbFile>() { TryGetLgb("level/bg.lgb"), TryGetLgb("level/planmap.lgb"), TryGetLgb("level/planevent.lgb") };
            this.LgbFiles = lgbFiles.Where(l => l != null).ToArray();
        }
        private Lgb.LgbFile TryGetLgb(string name) {
            var path = BasePath + name;
            if (Packs.TryGetFile(path, out var file))
                return new Lgb.LgbFile(file);
            return null;
        }
        #endregion
    }
}
