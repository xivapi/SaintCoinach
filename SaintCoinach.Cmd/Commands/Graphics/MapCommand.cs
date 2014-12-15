using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;
using G = SaintCoinach.Graphics;

namespace SaintCoinach.Cmd.Commands.Graphics {
    class MapCommand : ActionCommandBase {
        private ARealmReversed _Realm;

        public MapCommand(ARealmReversed realm)
            : base("map", "Display a map.") {
            _Realm = realm;
        }

        public async override Task<bool> InvokeAsync(string paramList) {
            if (string.IsNullOrWhiteSpace(paramList)) {
                OutputError("No map specified.");
                return false;
            }

            paramList = paramList.Trim();
            var map = _Realm.GameData.GetSheet<Xiv.Map>().FirstOrDefault(
                _ => _.PlaceName != null &&
                    (string.Equals(paramList, _.PlaceName.Name, StringComparison.OrdinalIgnoreCase)
                     || string.Equals(paramList, _.PlaceName.NameWithoutArticle, StringComparison.OrdinalIgnoreCase)));

            if (map == null) {
                OutputError("Map not found.");
                return false;
            }
            var terr = map.TerritoryType;
            if (terr == null) {
                OutputError("Map has no territory assigned.");
                return false;
            }
            var bgEnd = terr.Bg.IndexOf("/level/");
            if (bgEnd < 0) {
                OutputError("Map has no territory assigned.");
                return false;
            }
            var bg = "bg/" + terr.Bg.Substring(0, bgEnd);

            var component = new G.Map(_Realm.Packs, bg);
            GraphicsHelper.RunViewer(component, string.Format("{0} ({1})", map, bg));

            return true;
        }
    }
}
