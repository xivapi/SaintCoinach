﻿using System;
using System.Diagnostics;
using System.Linq;
using Tharga.Console;
using Tharga.Console.Commands;
using Tharga.Console.Commands.Base;

namespace SaintCoinach.Cmd {
    class Program {
        private static void Main(string[] args) {
            var dataPath = Properties.Settings.Default.DataPath;

            if (args.Length > 0) {
                dataPath = args[0];
                args = args.Skip(1).ToArray();
            }
            if (string.IsNullOrWhiteSpace(dataPath))
                dataPath = SearchForDataPaths().FirstOrDefault(p => System.IO.Directory.Exists(p));
            if (string.IsNullOrWhiteSpace(dataPath) || !System.IO.Directory.Exists(dataPath)) {
                Console.WriteLine($"Need data!  The path '{dataPath}' doesn't exist.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            var realm = new ARealmReversed(dataPath, Ex.Language.English, @"app_data.sqlite");
            realm.Packs.GetPack(new IO.PackIdentifier("exd", IO.PackIdentifier.DefaultExpansion, 0)).KeepInMemory = true;

            Console.WriteLine("Game version: {0}", realm.GameVersion);
            Console.WriteLine("Definition version: {0}", realm.DefinitionVersion);
            
            if (realm.IsUpdateAvailable()) {
                Console.Write("Update is available, download update (Y/n)? ");
                var updateQuery = Console.ReadLine();
                if (string.IsNullOrEmpty(updateQuery) || string.Equals("y", updateQuery, StringComparison.OrdinalIgnoreCase)) {
                    var time = Stopwatch.StartNew();
                    realm.UpdateDefinition();
                    Console.WriteLine($"Updated in {TimeSpan.FromMilliseconds(time.ElapsedMilliseconds):c}");
                    Console.WriteLine($"Please restart the application.");
                    return;
                } else
                    Console.WriteLine("Skipping update.");
            }
            
            var cns = new Tharga.Console.Consoles.ClientConsole();
            var cmd = new RootCommand(cns);

            Setup(cmd, realm);

            (new CommandEngine(cmd)).Start(args);
        }

        static void Setup(RootCommand rootCmd, ARealmReversed realm) {
            var assembly = typeof(Program).Assembly;
            foreach (var t in assembly.GetTypes().Where(t => typeof(AsyncActionCommandBase).IsAssignableFrom(t)))
            {
                var cons = t.GetConstructor(new[] { typeof(ARealmReversed) });
                rootCmd.RegisterCommand((AsyncActionCommandBase)cons.Invoke(new[] { realm }));
            }
        }

        static string[] SearchForDataPaths() {
            const string gameFolder = "FINAL FANTASY XIV - A Realm Reborn";

            string programDir;
            if (Environment.Is64BitProcess)
                programDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            else
                programDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            return new string[] {
                System.IO.Path.Combine(programDir, "SquareEnix", gameFolder),
                System.IO.Path.Combine(@"D:\Games\SteamApps\common", gameFolder)
            };
        }
    }
}
