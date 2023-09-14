using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using SaintCoinach.Xiv;
using SaintCoinach.Cmd.Commands;
using Tharga.Console.Commands.Base;

#pragma warning disable CS1998

/// <summary>
/// Class responsible for saving mdl and texture files for furniteres and housing yards.
/// </summary>
namespace SaintCoinach.Cmd.Commands {
    public class MdlCommand : AsyncActionCommandBase {
        private ARealmReversed _Realm;
        private HousingItem[] _AllFurniture;

        public MdlCommand(ARealmReversed realm)
            : base("furn_mdl_tex", "Export all MDL Furniture/Yard files.") {
            _Realm = realm;
        }

        /// <summary>
        /// Loads all furniture and yard information in order to export them.
        /// </summary>
        /// <param name="paramList">List of parameters being used in this function</param>
        /// <return name="result">A boolean indicating if the process failed or not.</return>
        public override async Task InvokeAsync(string[] paramList) {
            var indoor = _Realm.GameData.GetSheet("HousingFurniture");
            var outdoor = _Realm.GameData.GetSheet("HousingYardObject");
            _AllFurniture = indoor.Cast<HousingItem>().Concat(outdoor.Cast<HousingItem>()).Where(_ => _.Item != null && _.Item.Key != 0 && _.Item.Name.ToString().Length > 0).OrderBy(_ => _.Item.Name).ToArray();

            var successCount = 0;
            var failCount = 0;
            foreach (HousingItem outdoorItem in _AllFurniture) {
                var filePath = outdoorItem.GetScene().File.ToString();

                try {

                    if (string.IsNullOrWhiteSpace(filePath))
                        continue;

                    if (ExportFile(filePath)) {
                        ++successCount;
                    }
                    else {
                        OutputError($"File {filePath} not found.");
                        ++failCount;
                    }
                }
                catch (Exception e) {
                    OutputError("Export of {filePath} failed!");
                    OutputError(e, true);
                    ++failCount;
                }
            }
            OutputInformation($"{successCount} files exported, {failCount} failed");
        }

        /// <summary>
        /// Exports files to mdl and text using the function that exports any asset object type.
        /// </summary>
        /// <param name="filePath">File path to where the item's sgb file is located</param>
        /// <return name="result">A boolean indicating if the file was saved correctly.</return>
        private bool ExportFile(string filePath) {
            var result = false;
            var resultMdl = false;
            var resultTexD = false;
            var resultTexN = false;
            var resultTexS = false;

            try {
                RawCommand command = new RawCommand(_Realm);

                //Running for mdl
                resultMdl = WriteFunction(filePath.Replace("asset", "bgparts").Replace("sgb", "mdl"));

                //Running for all three textures
                resultTexD = WriteFunction(filePath.Replace("asset", "texture").Replace(".sgb", "_1a_d.tex"));
                resultTexN = WriteFunction(filePath.Replace("asset", "texture").Replace(".sgb", "_1a_n.tex"));
                resultTexS = WriteFunction(filePath.Replace("asset", "texture").Replace(".sgb", "_1a_s.tex"));

                // Only return true when mdl exports was sucessfull
                result = resultMdl;
            }
            catch (Exception e) {
                OutputError(e.Message);
                return result;
            }

            return result;
        }

        private bool WriteFunction(string filePath) {
            if (!_Realm.Packs.TryGetFile(filePath, out var file)) return false;
            try {
                var target = new FileInfo(Path.Combine(_Realm.GameVersion, file.Path));
                if (!target.Directory.Exists)
                    target.Directory.Create();

                var data = file.GetData();
                File.WriteAllBytes(target.FullName, data);
            }
            catch (Exception e) {
                OutputError(e.Message);
                return false;
            }

            return true;
        }

    }
}