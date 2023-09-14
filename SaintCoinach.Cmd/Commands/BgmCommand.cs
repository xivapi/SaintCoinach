using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tharga.Console.Commands.Base;

#pragma warning disable CS1998

namespace SaintCoinach.Cmd.Commands {
    public class BgmCommand : AsyncActionCommandBase {
        private ARealmReversed _Realm;

        public BgmCommand(ARealmReversed realm)
            : base("bgm", "Export all BGM files (default), or only those matching specific strings, separated by spaces (e.g. bgm_ride bgm_orch)") {
            _Realm = realm;
        }

        public override async Task InvokeAsync(string[] paramList) {
            var bgms = _Realm.GameData.GetSheet("BGM");
            string[] searchStrings;

            if (paramList.Length == 0)
                searchStrings = Array.Empty<String>();
            else
                searchStrings = paramList;

            var successCount = 0;
            var failCount = 0;
            foreach (Xiv.IXivRow bgm in bgms) {
                var filePath = bgm["File"].ToString();

                try {
                    if (string.IsNullOrWhiteSpace(filePath) || !IsMatch(searchStrings, filePath))
                        continue;

                    if (ExportFile(filePath, null)) {
                        ++successCount;
                    } else {
                        OutputError($"File {filePath} not found.");
                        ++failCount;
                    }
                } catch(Exception e) {
                    OutputError($"Export of {filePath} failed!");
                    OutputError(e, true);
                    ++failCount;
                }
            }

            var orchestrion = _Realm.GameData.GetSheet("Orchestrion");
            var orchestrionPath = _Realm.GameData.GetSheet("OrchestrionPath");
            foreach (Xiv.IXivRow orchestrionInfo in orchestrion) {
                var path = orchestrionPath[orchestrionInfo.Key];
                var name = orchestrionInfo["Name"].ToString();
                var filePath = path["File"].ToString();

                if (string.IsNullOrWhiteSpace(filePath) || !IsMatch(searchStrings, filePath))
                    continue;

                try {
                    if (ExportFile(filePath, name)) {
                        ++successCount;
                    } else {
                        OutputError($"File {filePath} not found.");
                        ++failCount;
                    }
                }
                catch (Exception e) {
                    OutputError($"Export of {filePath} failed!");
                    OutputError(e, true);
                    ++failCount;
                }
            }

            OutputInformation($"{successCount} files exported, {failCount} failed");
        }

        private bool ExportFile(string filePath, string suffix) {
            if (!_Realm.Packs.TryGetFile(filePath, out var file))
                return false;

            var scdFile = new Sound.ScdFile(file);
            var count = 0;
            for (var i = 0; i < scdFile.ScdHeader.EntryCount; ++i) {
                var e = scdFile.Entries[i];
                if (e == null)
                    continue;

                var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(filePath);
                if (suffix != null)
                    fileNameWithoutExtension += "-" + suffix;
                if (++count > 1)
                    fileNameWithoutExtension += "-" + count;

                foreach (var invalidChar in System.IO.Path.GetInvalidFileNameChars())
                    fileNameWithoutExtension = fileNameWithoutExtension.Replace(invalidChar.ToString(), "");

                var targetPath = System.IO.Path.Combine(_Realm.GameVersion, System.IO.Path.GetDirectoryName(filePath), fileNameWithoutExtension);

                switch (e.Header.Codec) {
                    case Sound.ScdCodec.MSADPCM:
                        targetPath += ".wav";
                        break;
                    case Sound.ScdCodec.OGG:
                        targetPath += ".ogg";
                        break;
                    default:
                        throw new NotSupportedException();
                }

                var fInfo = new System.IO.FileInfo(targetPath);

                if (!fInfo.Directory.Exists)
                    fInfo.Directory.Create();
                System.IO.File.WriteAllBytes(fInfo.FullName, e.GetDecoded());
            }

            return true;
        }

        private bool IsMatch(String[] searchStrings, string filePath) {
            return searchStrings.Length == 0 || searchStrings.Any(searchString => filePath.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) > 0);
        }
    }
}