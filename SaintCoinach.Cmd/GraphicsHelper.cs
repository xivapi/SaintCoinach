using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SaintCoinach.Cmd {
    static class GraphicsHelper {
        public static IEnumerable<string> ApplyMaterial(Graphics.Model model, string[] parts, int offset) {

            if (parts.Length > offset) {
                int v;
                if (int.TryParse(parts[offset], out v)) {
                    int? stain = null;
                    if (parts.Length > offset + 1) {
                        int temp;
                        if (int.TryParse(parts[offset + 1], out temp) && temp != 0)
                            stain = temp;
                    }

                    var stainAll = true;
                    var versionAll = true;

                    foreach (var mesh in model.Meshes) {
                        if (mesh.AvailableMaterialVersions.Contains(v))
                            mesh.MaterialVersion = v;
                        else
                            versionAll = false;
                        if (stain.HasValue && mesh.CanStain && mesh.AvailableStains.Contains(stain.Value))
                            mesh.MaterialStain = stain.Value;
                        else if (stain.HasValue)
                            stainAll = false;
                    }

                    if (!versionAll)
                        yield return ("Version not present on all materials, using default on some.");
                    if (!stainAll)
                        yield return ("Stain not present on all materials, using default on some.");
                } else
                    yield return ("Version input could not be parsed.");
            }
        }

        public static void RunViewer(Graphics.IComponent component, string title) {
            var thread = new Thread(RunViewerAsync);
            thread.IsBackground = true;
            thread.Name = "3D";
            thread.Start(Tuple.Create(component, title));
        }
        public static void RunViewer(Graphics.ViewerEngine engine) {
            var thread = new Thread(RunViewerDirectAsync);
            thread.IsBackground = true;
            thread.Name = "3D";
            thread.Start(engine);
        }

        static void RunViewerAsync(object state) {
            var t = (Tuple<Graphics.IComponent, string>)state;

            var e = new Graphics.ViewerEngine(t.Item2);

            e.Add(t.Item1);

            try {
                e.Run();
            } finally {
                GC.Collect();
            }
        }
        static void RunViewerDirectAsync(object state) {
            var e = (Graphics.ViewerEngine)state;
            try {
                e.Run();
            } finally {
                GC.Collect();
            }
        }
    }
}
