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

            var leves = realm.GameData.GetSheet<Xiv.Leve>();
            var craftLeves = realm.GameData.GetSheet<Xiv.CraftLeve>();
            var levesOfCrafting = leves.Where(_ => _.ClassJobCategory.ClassJobs.Any(cj => cj.Abbreviation == "FSH") && _.CharacterLevel == 45).OrderBy(_ => _.LeveAssignmentType.Key).GroupBy(_ => _.LeveAssignmentType);
            foreach (var levesOfCraftingGroup in levesOfCrafting) {
                System.Diagnostics.Trace.WriteLine(levesOfCraftingGroup.Key.Name);
                foreach (var leveOfCrafting in levesOfCraftingGroup) {
                    var dataId = leveOfCrafting.DataId;
                    var craftLeve = craftLeves.FirstOrDefault(_ => _.Key == dataId);
                    if (craftLeve == null)
                        continue;
                    /*System.Diagnostics.Trace.WriteLine(string.Format("    {0}: {1}", leveOfCrafting.Name,
                        string.Join("; ", craftLeve.Items.Select(_ => string.Format("{0} {1}", _.Count, _.Item)))));*/
                    System.Diagnostics.Trace.WriteLine(string.Format("  {0}", leveOfCrafting.Name));
                    foreach (var item in craftLeve.Items) {
                        System.Diagnostics.Trace.WriteLine(string.Format("    {0} {1}", item.Count, item.Item));
                        var ii = item.Item as Xiv.InventoryItem;
                        var r = ii.RecipesAsResult.FirstOrDefault();
                        if (r != null) {
                            System.Diagnostics.Trace.WriteLine(string.Format("      {0} {1}", r.ClassJob, r.RecipeLevel.RecipeLevelTable.Key));
                            foreach (var mat in r.Ingredients.Where(_ => _.Type != Xiv.RecipeIngredientType.Crystal)) {
                                System.Diagnostics.Trace.WriteLine(string.Format("      {0} {1}", mat.Count, mat.Item));
                            }
                        }
                    }
                }
            }

            var cmd = new RootCommand();

            Setup(cmd, realm);

            (new CommandEngine(cmd)).Run(args);
        }
        static void WriteAll(StreamWriter writer, string type, string[] texts) {
            writer.WriteLine("|-");
            writer.WriteLine("! {0}", type);
            foreach (var t in texts)
                writer.WriteLine("| {0}", t);

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
