using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Exports {
    public class ModelExport {
        public Model Model { get; private set; }
        public List<byte[]> ExportedMeshes { get; private set; }

        private ModelExport() { }

        public static ModelExport AsObj(Model model, ImcVariant variant) {
            var export = new ModelExport();
            export.Model = model;

            export.ExportedMeshes = new List<byte[]>();
            foreach (var mesh in model.Meshes) {
                export.ExportedMeshes.Add(Obj.GetBytes(mesh));

                var material = mesh.Material.Get(variant);
            }

            return export;
        }
    }
}
