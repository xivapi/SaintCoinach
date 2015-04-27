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
    class ModelCommand : ActionCommandBase {
        private ARealmReversed _Realm;

        public ModelCommand(ARealmReversed realm)
            : base("model", "Display a model.") {
                _Realm = realm;
        }

        public async override Task<bool> InvokeAsync(string paramList) {
            if (string.IsNullOrWhiteSpace(paramList)) {
                OutputError("No model specified.");
                return false;
            }

            var splitParam = paramList.Split(' ');

            IO.File file;
            if (_Realm.Packs.TryGetFile(splitParam[0], out file)) {
                var mdlFile = file as G.Assets.ModelFile;
                if (mdlFile != null) {
                    var mdl = mdlFile.GetModel();
                    var subMdl = mdl.GetSubModel(0);

                    var component = new G.Model(subMdl);
                    foreach (var msg in GraphicsHelper.ApplyMaterial(component, splitParam, 1))
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
