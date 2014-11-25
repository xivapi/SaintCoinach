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
            const string DataPath = @"C:\Program Files (x86)\SquareEnix\FINAL FANTASY XIV - A Realm Reborn\game\sqpack\ffxiv";

            var packColl = new IO.PackCollection(DataPath);
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
        }
    }
}
