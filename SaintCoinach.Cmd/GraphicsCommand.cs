using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;

namespace SaintCoinach.Cmd {
    public class GraphicsCommand : ActionCommandBase {
        private IO.PackCollection _Pack;
        private Xiv.XivCollection _Data;

        public GraphicsCommand(IO.PackCollection pack, Xiv.XivCollection data)
            : base("3d", "Just for testing.") {
            _Pack = pack;
            _Data = data;
        }

        public async override Task<bool> InvokeAsync(string paramList) {
            var thread = new Thread(RunEngineAsync);
            thread.IsBackground = true;
            thread.Name = "3D";

            var eng = new Graphics.Viewer.ViewerEngine("Rawr");

            thread.Start(eng);

            return true;
        }

        static void RunEngineAsync(object state) {
            ((Graphics.Viewer.ViewerEngine)state).Run();
        }
    }
}
