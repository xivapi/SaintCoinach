using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Assets {
    public class Material {
        #region Fields
        private Model _Model;
        private string _Name;
        private string _VersionFormat;
        private int[] _AvailableVersions;
        #endregion

        #region Properties
        public Model Model { get { return _Model; } }
        public string Name { get { return _Name; } }
        public IEnumerable<int> AvailableVersions { get { return _AvailableVersions; } }
        #endregion

        #region Constructor
        public Material(Model model, string name) {
            _Model = model;
            _Name = name;

            CheckAvailableVersions();
        }
        #endregion

        #region Get
        public MaterialVersion GetVersion(int version) {
            return new MaterialVersion(this, version, string.Format(_VersionFormat, version));
        }
        #endregion

        #region Build
        private void CheckAvailableVersions() {
            var available = new List<int>();

            const int MinimumCheck = 1;
            const int MaximumCheck = 9999;
            const int Default = 0;

            var packColl = Model.File.Directory.Pack.Collection;
            if (packColl.FileExists(Name)) {
                _VersionFormat = Name;
                available.Add(Default);
            } else {
                var path = _Name.Replace("/material/material/", "/material/v{0:D4}/").Replace("/material/mt_", "/material/v{0:D4}/mt_");
                if (path.StartsWith("/"))
                    path = ExpandPath(path);

                var isVersioned = path.Contains("{0");
                _VersionFormat = path;
                if (isVersioned) {
                    for (int i = MinimumCheck; i <= MaximumCheck; ++i) {
                        var fileName = string.Format(_VersionFormat, i);
                        if (packColl.FileExists(fileName))
                            available.Add(i);
                    }
                } else
                    available.Add(Default);
            }

            _AvailableVersions = available.ToArray();
        }
        #endregion

        #region Helpers
        private static Dictionary<Tuple<char, char>, string> MaterialTypeMappings = new Dictionary<Tuple<char, char>, string> {
            { Tuple.Create('c', 'b'), "chara/human/c{0:D4}/obj/body/b{1:D4}/material/mt_c{0:D4}b{1:D4}_a.mtrl" },
            { Tuple.Create('c', 'f'), "chara/human/c{0:D4}/obj/face/f{1:D4}/material/mt_c{0:D4}f{1:D4}_fac_a.mtrl" },
            { Tuple.Create('c', 'h'), "chara/human/c{0:D4}/obj/hair/h{1:D4}/material/v{{0:D4}}/mt_c{0:D4}h{1:D4}_hir_a.mtrl" },
            { Tuple.Create('m', 'b'), "chara/monster/m{0:D4}/obj/body/b{1:D4}/material/v{{0:D4}}/mt_m{0:D4}b{1:D4}_a.mtrl" },
        };
        private static Regex MaterialTypePattern = new Regex(@"[_/](?<t1>[a-z])(?<v1>\d{4})(?<t2>[a-z])(?<v2>\d{4})[_/\.]", RegexOptions.Compiled);
        private string ExpandPath(string path) {
            var modelPath = Model.File.Path;

            var match = MaterialTypePattern.Match(path);
            if (match.Success) {
                int v1 = int.Parse(match.Groups["v1"].Value);
                int v2 = int.Parse(match.Groups["v2"].Value);
                var key = Tuple.Create(match.Groups["t1"].Value[0], match.Groups["t2"].Value[0]);

                string targetFormat;
                if (MaterialTypeMappings.TryGetValue(key, out targetFormat)) {
                    path = string.Format(targetFormat, v1, v2);     // No versions of these
                } else {
                    var last = modelPath.LastIndexOf('/');
                    var secondToLast = modelPath.LastIndexOf('/', last - 1);
                    path = modelPath.Substring(0, secondToLast) + "/material/v{0:D4}" + path;
                }
            } else if (path != modelPath)
                return ExpandPath(modelPath);
            else
                throw new NotSupportedException(string.Format("Unable to expand material file path '{0}':'{1}'.", modelPath, path));

            return path;
        }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
