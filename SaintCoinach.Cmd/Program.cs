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
            rootCmd.RegisterCommand(new Commands.LanguageCommand(realm));
            rootCmd.RegisterCommand(new Commands.RawCommand(realm));
            rootCmd.RegisterCommand(new Commands.ImageCommand(realm));
            rootCmd.RegisterCommand(new Commands.UiCommand(realm));
            rootCmd.RegisterCommand(new Commands.ExdCommand(realm));
            rootCmd.RegisterCommand(new Commands.BgmCommand(realm));

            rootCmd.RegisterCommand(new Commands.GraphicsCommand(realm));
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
