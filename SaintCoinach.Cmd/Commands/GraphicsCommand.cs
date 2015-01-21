using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;

namespace SaintCoinach.Cmd.Commands {
    public class GraphicsCommand : ContainerCommandBase {
        public GraphicsCommand(ARealmReversed realm)
            : base("3d") {
                SubCommands.Add(new Graphics.ModelCommand(realm));
                SubCommands.Add(new Graphics.TerritoryCommand(realm));
                SubCommands.Add(new Graphics.MapCommand(realm));
                SubCommands.Add(new Graphics.EquipmentCommand(realm));
                SubCommands.Add(new Graphics.MonsterCommand(realm));
        }
    }
}
