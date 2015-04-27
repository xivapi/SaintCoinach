using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;
using G = SaintCoinach.Graphics;

namespace SaintCoinach.Cmd.Commands.Graphics {
    class MonsterCommand : ActionCommandBase {
        private ARealmReversed _Realm;

        public MonsterCommand(ARealmReversed realm)
            : base("monster", "Display a monster.") {
            _Realm = realm;
        }

        public async override Task<bool> InvokeAsync(string paramList) {
            if (string.IsNullOrWhiteSpace(paramList)) {
                OutputError("No model specified.");
                return false;
            }

            var splitParam = paramList.Split(' ');
            int m;
            int b = 1, v = 1;
            if (!int.TryParse(splitParam[0], out m)) {
                OutputError("Unable to parse input.");
                return false;
            }
            if (splitParam.Length > 1 && !int.TryParse(splitParam[1], out b)) {
                OutputError("Unable to parse input.");
                return false;
            }

            var mdlName = string.Format("chara/monster/m{0:D4}/obj/body/b{1:D4}/model/m{0:D4}b{1:D4}.mdl", m, b);
            IO.File file;
            if (_Realm.Packs.TryGetFile(mdlName, out file)) {
                var mdlFile = file as G.Assets.ModelFile;
                if (mdlFile != null) {
                    var mdl = mdlFile.GetModel();
                    var subMdl = mdl.GetSubModel(0);

                    var component = new G.Model(subMdl);
                    foreach (var msg in GraphicsHelper.ApplyMaterial(component, splitParam, 2))
                        OutputWarning(msg);

                    GraphicsHelper.RunViewer(component, file.Path);
                } else
                    OutputError("File is not a model file.");
            } else
                OutputError("File could not be found.");

            return true;
        }
    }
}
