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
    class TerritoryCommand : ActionCommandBase {
        private ARealmReversed _Realm;

        public TerritoryCommand(ARealmReversed realm)
            : base("territory", "Display territory.") {
            _Realm = realm;
        }

        public async override Task<bool> InvokeAsync(string paramList) {
            if (string.IsNullOrWhiteSpace(paramList)) {
                OutputError("No territory specified.");
                return false;
            }
            var component = new G.Map(_Realm.Packs, paramList);
            GraphicsHelper.RunViewer(component, paramList);

            return true;
        }
    }
}
