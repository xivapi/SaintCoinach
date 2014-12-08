using System;
using System.Collections.Generic;
using System.IO;
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
            if (string.IsNullOrWhiteSpace(dataPath))
                dataPath = SearchForDataPath();
            if (string.IsNullOrWhiteSpace(dataPath) || !System.IO.Directory.Exists(dataPath)) {
                Console.WriteLine("Need data!");
                return;
            }

            var realm = new ARealmReversed(dataPath, Ex.Language.English);

            /*var useDir = Path.Combine(Directory.GetCurrentDirectory(), realm.GameVersion);
            if (!Directory.Exists(useDir))
                Directory.CreateDirectory(useDir);
            Directory.SetCurrentDirectory(useDir);*/

            var cmd = new RootCommand();

            Setup(cmd, realm);

            (new CommandEngine(cmd)).Run(args);
        }

        static void Setup(RootCommand rootCmd, ARealmReversed realm) {
            var packColl = realm.Packs;
            var dataColl = realm.GameData;

            rootCmd.RegisterCommand(new RawCommand(packColl));
            rootCmd.RegisterCommand(new ImageCommand(packColl));
            rootCmd.RegisterCommand(new UiCommand(packColl));
            rootCmd.RegisterCommand(new ExdCommand(dataColl));
            rootCmd.RegisterCommand(new BgmCommand(packColl, dataColl));

            rootCmd.RegisterCommand(new GraphicsCommand(packColl, dataColl));

            rootCmd.RegisterCommand(new TempCommand(packColl, dataColl));
        }

        static string SearchForDataPath() {
            string programDir;
            if(Environment.Is64BitProcess)
                programDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            else
                programDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            return System.IO.Path.Combine(programDir, "SquareEnix", "FINAL FANTASY XIV - A Realm Reborn");
        }
    }
}
