using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;

namespace SaintCoinach.Cmd.Commands {
    public class BgmCommand : ActionCommandBase {
        private ARealmReversed _Realm;

        public BgmCommand(ARealmReversed realm)
            : base("bgm", "Export all BGM files.") {
            _Realm = realm;
        }

        public override async Task<bool> InvokeAsync(string paramList) {
            var bgms = _Realm.GameData.GetSheet("BGM");

            var successCount = 0;
            var failCount = 0;
            foreach (Xiv.IXivRow bgm in bgms) {
                var filePath = bgm["File"].ToString();

                try {

                    if (string.IsNullOrWhiteSpace(filePath))
                        continue;

                    IO.File file;

                    if (_Realm.Packs.TryGetFile(filePath, out file)) {

                        var scdFile = new Sound.ScdFile(file);
                        for (var i = 0; i < scdFile.ScdHeader.EntryCount; ++i) {
                            var e = scdFile.Entries[i];
                            if (e == null)
                                continue;

                            var targetPath = System.IO.Path.Combine(_Realm.GameVersion, filePath + "-" + i.ToString());

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

                        ++successCount;
                    } else {
                        OutputError("File {0} not found.", filePath);
                        ++failCount;
                    }
                } catch(Exception e) {
                    OutputError("Export of {0} failed: {1}", filePath, e.Message);
                    ++failCount;
                }
            }
            OutputInformation("{0} files exported, {1} failed", successCount, failCount);

            return true;
        }
    }
}
