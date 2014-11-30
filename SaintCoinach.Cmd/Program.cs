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
            if (string.IsNullOrWhiteSpace(dataPath))
                dataPath = SearchForDataPath();
            if (string.IsNullOrWhiteSpace(dataPath) || !System.IO.Directory.Exists(dataPath)) {
                Console.WriteLine("Need data!");
                return;
            }

            var realm = new ARealmReversed(dataPath, Ex.Language.English);

            using (var log = new System.IO.StreamWriter("Shops.log", false, Encoding.UTF8)) {
                foreach (var shop in realm.GameData.Shops) {
                    Console.WriteLine("#{0}: {1}", shop.Key, shop.Name);
                    var items = shop.ShopItems;
                    if (items.Any()) {
                        log.WriteLine("#{0}: {1}", shop.Key, shop.Name);
                        foreach (var npc in shop.ENpcs) {
                            log.WriteLine(" > {0}", npc.Resident.Singular);
                            foreach (var level in npc.Levels)
                                log.WriteLine("   > {0} ({1} / {2})", level.Map.PlaceName, level.MapX, level.MapY);
                        }

                        foreach (var item in items) {
                            log.WriteLine(" - {0} {1}", item.Count, item.Item);
                            foreach (var cost in item.Costs)
                                log.WriteLine("   - {0} {1} (HQ: {2})", cost.Count, cost.Item, cost.RequireHq);
                        }
                    }
                }
            }
            return;

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
