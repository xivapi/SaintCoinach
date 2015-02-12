using System;
using System.Collections.Generic;
using System.Linq;

namespace SaintCoinach.Graphics.Assets {
    public static class VertexReader {
        #region Static

        private static readonly Dictionary<int, VertexDataReader> DataReaders = new Dictionary<int, VertexDataReader> {
            {
                0x0C, VertexDataReader.NormalTexture
            }, {
                0x10, VertexDataReader.NormalTextureBlend
            }, {
                0x14, VertexDataReader.NormalTextureColorBlend
            }, {
                0x18, VertexDataReader.NormalTextureColor
            }, {
                0x1C, VertexDataReader.NormalTextureColorBlend2
            }
        };

        #endregion

        public static VertexBase[] Read(Mesh mesh, byte[] data, out Type vertexType) {
            VertexDataReader dataReader;
            if (!DataReaders.TryGetValue(mesh.Header.BytesPerVertexData, out dataReader))
                throw new NotSupportedException(string.Format("Unsupported vertex data length {0:X2}h",
                    mesh.Header.BytesPerVertexData));
            vertexType = dataReader.VertexType;

            Func<byte[], int, Vector4> positionReader;
            //int positionLength;
            switch (mesh.Type) {
                case 0x000E0000:
                    positionReader = ReadHalfVector4;
                    //positionLength = 0x08;
                    break;
                case 0x00030000:
                case 0x00020000:
                    // XXX: I have no idea what the difference here is
                    positionReader = ReadSingleVector4;
                    //positionLength = 0x10;
                    break;
                default:
                    throw new NotSupportedException(string.Format("Unsupported mesh type {0:X8}h", mesh.Type));
            }

            var positionOffset = mesh.Header.VertexOffset;
            var dataOffset = positionOffset + mesh.Header.VertexCount * mesh.Header.BytesPerVertexPosition;

            var vertices = new VertexBase[mesh.Header.VertexCount];
            for (var i = 0; i < mesh.Header.VertexCount; ++i) {
                var vertex = (VertexBase)Activator.CreateInstance(dataReader.VertexType);

                vertex.Position = positionReader(data, positionOffset);
                // Bones after position? (offset + positionLength)
                dataReader.Fill(vertex, data, dataOffset);

                vertices[i] = vertex;

                positionOffset += mesh.Header.BytesPerVertexPosition;
                dataOffset += mesh.Header.BytesPerVertexData;
            }

            return vertices.ToArray();
        }

        private static Vector4 ReadHalfVector4(byte[] data, int offset) {
            var x = HalfHelper.Unpack(data, offset + 0x00);
            var y = HalfHelper.Unpack(data, offset + 0x02);
            var z = HalfHelper.Unpack(data, offset + 0x04);
            var w = 1; //HalfHelper.Unpack(data, offset + 0x06);
            return new Vector4(x, y, z, w);
        }

        private static Vector4 ReadSingleVector4(byte[] data, int offset) {
            var x = BitConverter.ToSingle(data, offset + 0x00);
            var y = BitConverter.ToSingle(data, offset + 0x04);
            var z = BitConverter.ToSingle(data, offset + 0x08);
            var w = 1; // BitConverter.ToSingle(data, offset + 0x0C);
            return new Vector4(x, y, z, w);
        }
    }
}
