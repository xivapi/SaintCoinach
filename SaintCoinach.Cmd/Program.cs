using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;

namespace SaintCoinach.Cmd {
    class Program {
        static void Main(string[] args) {
            var dataPath = Properties.Settings.Default.DataPath;

            if (args.Length > 0) {
                dataPath = args[0];
                args = new string[0];
            }
            if (!System.IO.Directory.Exists(dataPath)) {
                Console.WriteLine("Need data!");
                return;
            }

            var packColl = new IO.PackCollection(dataPath);
            var dataColl = new Xiv.XivCollection(packColl);
            dataColl.ActiveLanguage = Ex.Language.English;
            dataColl.Definition = Ex.Relational.Definition.RelationDefinition.Deserialize("ex.yaml");
            dataColl.Definition.Compile();

            var cmd = new RootCommand();

            Setup(cmd, packColl, dataColl);

            (new CommandEngine(cmd)).Run(args);
        }

        static void Setup(RootCommand rootCmd, IO.PackCollection packColl, Xiv.XivCollection dataColl) {
            rootCmd.RegisterCommand(new RawCommand(packColl));
            rootCmd.RegisterCommand(new ImageCommand(packColl));
            rootCmd.RegisterCommand(new UiCommand(packColl));
            rootCmd.RegisterCommand(new ExdCommand(dataColl));
            rootCmd.RegisterCommand(new BgmCommand(packColl, dataColl));

            rootCmd.RegisterCommand(new GraphicsCommand(packColl, dataColl));
        }
    }
}
