using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;

namespace SaintCoinach.Cmd.Commands {
    public class MapCommand : ActionCommandBase {
        private ARealmReversed _Realm;

        public MapCommand(ARealmReversed realm)
            : base("maps", "Export all map images.") {
            _Realm = realm;
        }

        public override async Task<bool> InvokeAsync(string paramList) {

            var c = 0;
            var allMaps = _Realm.GameData.GetSheet<SaintCoinach.Xiv.Map>();
            foreach(var map in allMaps) {
                if (map.TerritoryType == null || map.TerritoryType.Key == 0)
                    continue;
                var img = map.MediumImage;
                if (img == null)
                    continue;

                var outPathSb = new StringBuilder("ui/map/");
                var territoryName = map.TerritoryType.Name.ToString();
                if(territoryName.Length < 3) {
                    outPathSb.AppendFormat("{0}/", territoryName);
                } else {
                    outPathSb.AppendFormat("{0}/", territoryName.Substring(0, 3));
                }

                outPathSb.AppendFormat("{0} - ", territoryName);
                outPathSb.AppendFormat("{0}", ToPathSafeString(map.PlaceName.Name.ToString()));
                if (map.LocationPlaceName != null && map.LocationPlaceName.Key != 0 && !map.LocationPlaceName.Name.IsEmpty)
                    outPathSb.AppendFormat(" - {0}", ToPathSafeString(map.LocationPlaceName.Name.ToString()));
                outPathSb.Append(".png");

                var outFile = new FileInfo(Path.Combine(_Realm.GameVersion, outPathSb.ToString()));
                if (!outFile.Directory.Exists)
                    outFile.Directory.Create();

                img.Save(outFile.FullName);
                ++c;
            }
            OutputInformation("{0} maps saved", c);

            return true;
        }
        static string ToPathSafeString(string input, char invalidReplacement = '_') {
            var sb = new StringBuilder(input);
            var invalid = Path.GetInvalidFileNameChars();
            foreach (var c in invalid)
                sb.Replace(c, invalidReplacement);
            return sb.ToString();
        }
    }
}
