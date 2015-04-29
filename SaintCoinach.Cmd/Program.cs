using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;

namespace SaintCoinach.Cmd {
    class ConsoleProgressReporter : IProgress<Ex.Relational.Update.UpdateProgress> {

        #region IProgress<UpdateProgress> Members

        public void Report(Ex.Relational.Update.UpdateProgress value) {
            Console.WriteLine(value);
        }

        #endregion
    }
    class Program {
        private static void Main(string[] args) {
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

            var realm = new ARealmReversed(dataPath, @"SaintCoinach.History.zip", Ex.Language.English, @"app_data.sqlite");
            realm.Packs.GetPack(new IO.PackIdentifier("exd", IO.PackIdentifier.DefaultExpansion, 0)).KeepInMemory = true;

            Console.WriteLine("Game version: {0}", realm.GameVersion);
            Console.WriteLine("Definition version: {0}", realm.DefinitionVersion);

            if (!realm.IsCurrentVersion) {
                Console.Write("Update is available, perform update (Y/n)? ");
                var updateQuery = Console.ReadLine();
                if (string.Equals("y", updateQuery, StringComparison.OrdinalIgnoreCase)) {
                    realm.Update(true, new ConsoleProgressReporter());
                } else
                    Console.WriteLine("Skipping update");
            }

            var cns = new Tharga.Toolkit.Console.Command.Base.ClientConsole();
            var cmd = new RootCommand(cns);

            Setup(cmd, realm);

            (new CommandEngine(cmd)).Run(args);
        }

        static void Setup(RootCommand rootCmd, ARealmReversed realm) {
            rootCmd.RegisterCommand(new Commands.LanguageCommand(realm));
            rootCmd.RegisterCommand(new Commands.RawCommand(realm));
            rootCmd.RegisterCommand(new Commands.ImageCommand(realm));
            rootCmd.RegisterCommand(new Commands.UiCommand(realm));
            rootCmd.RegisterCommand(new Commands.ExdCommand(realm));
            rootCmd.RegisterCommand(new Commands.RawExdCommand(realm));
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
