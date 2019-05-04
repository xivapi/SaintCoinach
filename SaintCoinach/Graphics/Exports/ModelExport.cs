using SaintCoinach.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Exports {
    public class ModelExport {
        public Model Model { get; private set; }
        public ImcVariant Variant { get; private set; }
        public List<MeshExport> Meshes { get; private set; }

        public string GetErrors() {
            return string.Join(Environment.NewLine, Meshes.SelectMany(m => m.Errors));
        }

        private ModelExport() { }

        public static ModelExport AsObj(Model model, ImcVariant variant) {
            var export = new ModelExport();
            export.Model = model;
            export.Variant = variant;

            export.Meshes = new List<MeshExport>();
            foreach (var mesh in model.Meshes) {
                var bytes = Obj.GetBytes(mesh);
                var material = mesh.Material.Get(variant);
                export.Meshes.Add(new MeshExport(bytes, material));
            }
            return export;
        }
    }

    public class MeshExport {
        public byte[] Bytes { get; private set; }
        public ImageFile Diffuse { get; private set; }
        public ImageFile Specular { get; private set; }
        public ImageFile Normal { get; private set; }
        public ImageFile Mask { get; private set; }
        public ImageFile Table { get; private set; }
        public ImageFile Emissive { get; private set; }
        public ImageFile Alpha { get; private set; }
        public string[] Errors { get; private set; }

        public MeshExport(byte[] bytes, Material material) {
            Bytes = bytes;

            ExportTextures(material);
        }

        private void ExportTextures(Material material) {
            var errors = new List<string>();

            foreach (var param in material.TextureParameters) {
                var tex = material.TexturesFiles[param.TextureIndex];

                switch (param.ParameterId) {
                    case 0x0C5EC1F1:
                        Normal = tex;
                        break;

                    case 0x2005679F:
                        Table = tex;
                        break;

                    case 0x8A4E82B6:
                        Mask = tex;
                        break;

                    case 0x115306BE:
                        Diffuse = tex;
                        break;

                    case 0x2B99E025:
                        Specular = tex;
                        break;

                    // todo: emissive
                    // todo: alpha

                    default:
                        errors.Add($"Unknown character parameter {param.ParameterId:X8} for texture '{tex.Path}' in material '{material.File.Path}'");
                        break;
                }
            }

            Errors = errors.ToArray();
        }
    }
}
