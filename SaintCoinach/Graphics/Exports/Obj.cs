using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Exports {
    public static class Obj {
        public static byte[] GetBytes(Mesh mesh) {
            var sb = new StringBuilder();

            // Positions
            foreach (var vertex in mesh.Vertices.Where(v => v.Position.HasValue)) {
                var pos = vertex.Position.Value;
                sb.AppendLine($"v {pos.X:N5} {pos.Y:N5} {pos.Z:N5}");
            }

            // UVs
            foreach (var vertex in mesh.Vertices.Where(v => v.UV.HasValue)) {
                var uv = vertex.UV.Value;
                sb.AppendLine($"vt {uv.X:N5} {(1 - uv.Y):N5}");
            }

            // Normals
            foreach (var vertex in mesh.Vertices.Where(v => v.Normal.HasValue)) {
                var normal = vertex.Normal.Value;
                sb.AppendLine($"vn {normal.X:N5} {normal.Y:N5} {normal.Z:N5}");
            }

            // Indices
            for (var i = 0; i < mesh.Indices.Length; i += 3) {
                var index1 = mesh.Indices[i] + 1;
                var index2 = mesh.Indices[i + 1] + 1;
                var index3 = mesh.Indices[i + 2] + 1;
                sb.AppendLine($"f {index1}/{index1}/{index1} {index2}/{index2}/{index2} {index3}/{index3}/{index3}");
            }

            return Encoding.ASCII.GetBytes(sb.ToString());
        }
    }
}
