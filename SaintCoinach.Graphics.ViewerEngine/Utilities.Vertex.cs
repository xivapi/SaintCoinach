using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace SaintCoinach.Graphics {
    public static partial class Utilities {
        public static class Vertex {
            #region Source
            static readonly VertexDataSource<Primitives.VertexCommon> DataSourceCommon = new VertexDataSourceCommon();
            static readonly VertexDataSource<Primitives.VertexDualTexture> DataSourceDualTexture = new VertexDataSourceDualTexture();

            abstract class VertexDataSource<T> {
                public abstract Vector4 GetPosition(T vertex);
                public abstract Vector3 GetNormal(T vertex);
                public abstract Vector2 GetWeight(T vertex);

                public abstract void SetPosition(ref T vertex, Vector4 value);
                public abstract void SetTangent(ref T vertex, Vector3 value);
                public abstract void SetBinormal(ref T vertex, Vector3 value);

                public abstract void FlipWinding(ref T vertex);
            }

            #region Common
            class VertexDataSourceCommon : VertexDataSource<Primitives.VertexCommon> {
                public override Vector4 GetPosition(Primitives.VertexCommon vertex) {
                    return vertex.Position;
                }
                public override Vector3 GetNormal(Primitives.VertexCommon vertex) {
                    return vertex.Normal;
                }
                public override Vector2 GetWeight(Primitives.VertexCommon vertex) {
                    return vertex.TextureCoordinates;
                }

                public override void SetPosition(ref Primitives.VertexCommon vertex, Vector4 value) {
                    vertex.Position = value;
                }
                public override void SetTangent(ref Primitives.VertexCommon vertex, Vector3 value) {
                    vertex.Tangent = value;
                }
                public override void SetBinormal(ref Primitives.VertexCommon vertex, Vector3 value) {
                    vertex.Binormal = value;
                }

                public override void FlipWinding(ref Primitives.VertexCommon vertex) {
                    vertex.TextureCoordinates.X = (1.0f - vertex.TextureCoordinates.X);
                }
            }
            #endregion

            #region Dual
            class VertexDataSourceDualTexture : VertexDataSource<Primitives.VertexDualTexture> {
                public override Vector4 GetPosition(Primitives.VertexDualTexture vertex) {
                    return vertex.Position;
                }
                public override Vector3 GetNormal(Primitives.VertexDualTexture vertex) {
                    return vertex.Normal;
                }
                public override Vector2 GetWeight(Primitives.VertexDualTexture vertex) {
                    var w1 = 1 - vertex.BlendWeight.W;
                    var w2 = vertex.BlendWeight.W;
                    return vertex.TextureCoordinates0 * w1 + vertex.TextureCoordinates1 * w2;
                }

                public override void SetPosition(ref Primitives.VertexDualTexture vertex, Vector4 value) {
                    vertex.Position = value;
                }
                public override void SetTangent(ref Primitives.VertexDualTexture vertex, Vector3 value) {
                    vertex.Tangent = value;
                }
                public override void SetBinormal(ref Primitives.VertexDualTexture vertex, Vector3 value) {
                    vertex.Binormal = value;
                }
                public override void FlipWinding(ref Primitives.VertexDualTexture vertex) {
                    vertex.TextureCoordinates0.X = (1.0f - vertex.TextureCoordinates0.X);
                    vertex.TextureCoordinates1.X = (1.0f - vertex.TextureCoordinates1.X);
                }
            }
            #endregion
            #endregion


            public static Primitives.VertexCommon[] Convert(Assets.VertexBase[] vertices, ushort[] indices) {
                var converted = vertices.Select(_ => new Primitives.VertexCommon {
                    Position = new Vector4(_.Position.X, _.Position.Y, _.Position.Z, _.Position.W),
                    Normal = Vector3.Normalize(new Vector3(_.Normal.X, _.Normal.Y, _.Normal.Z)),
                    TextureCoordinates = _.TextureCoordinates0.ToDX()
                }).ToArray();

                ReverseWinding(converted, indices, DataSourceCommon);

                Array.Reverse(indices);

                CalculateTangents(converted, indices, DataSourceCommon);

                return converted;
            }
            public static Primitives.VertexDualTexture[] Convert(Assets.VertexBlend[] vertices, ushort[] indices) {
                var converted = vertices.Select(_ => new Primitives.VertexDualTexture {
                    Position = new Vector4(_.Position.X, _.Position.Y, _.Position.Z, _.Position.W),
                    Normal = Vector3.Normalize(new Vector3(_.Normal.X, _.Normal.Y, _.Normal.Z)),
                    TextureCoordinates0 = _.TextureCoordinates0.ToDX(),
                    TextureCoordinates1 = _.TextureCoordinates1.ToDX(),
                    BlendWeight = _.BlendWeight.ToDX()
                }).ToArray();

                ReverseWinding(converted, indices, DataSourceDualTexture);

                Array.Reverse(indices);

                CalculateTangents(converted, indices, DataSourceDualTexture);

                return converted;
            }

            private static void ReverseWinding<T>(T[] vertices, ushort[] indices, VertexDataSource<T> dataSource) {
                for (var i = 0; i < indices.Length; i += 3) {
                    var swap = indices[i];
                    indices[i] = indices[i + 2];
                    indices[i + 2] = swap;
                }
                /*for (var i = 0; i < vertices.Length; ++i)
                    dataSource.FlipWinding(ref vertices[i]);*/
            }

            private static void CalculateTangents<T>(T[] vertices, ushort[] indices, VertexDataSource<T> dataSource) {
                var tan1 = new Vector3[vertices.Length];
                var tan2 = new Vector3[vertices.Length];
                for (int a = 0; a < tan1.Length; ++a) {
                    tan1[a] = new Vector3();
                    tan2[a] = new Vector3();
                }

                for (var a = 0; a + 3 <= indices.Length; a += 3) {
                    var i1 = indices[a];
                    var i2 = indices[a + 1];
                    var i3 = indices[a + 2];

                    var vert1 = vertices[i1];
                    var vert2 = vertices[i2];
                    var vert3 = vertices[i3];

                    var v1 = dataSource.GetPosition(vert1);
                    var v2 = dataSource.GetPosition(vert2);
                    var v3 = dataSource.GetPosition(vert3);

                    var w1 = dataSource.GetWeight(vert1);
                    var w2 = dataSource.GetWeight(vert2);
                    var w3 = dataSource.GetWeight(vert3);

                    var x1 = v2.X - v1.X;
                    var x2 = v3.X - v1.X;
                    var y1 = v2.Y - v1.Y;
                    var y2 = v3.Y - v1.Y;
                    var z1 = v2.Z - v1.Z;
                    var z2 = v3.Z - v1.Z;

                    var s1 = w2.X - w1.X;
                    var s2 = w3.X - w1.X;
                    var t1 = w2.Y - w1.Y;
                    var t2 = w3.Y - w1.Y;

                    var r = 1f / (s1 * t2 - s2 * t1);
                    var sdir = r * new Vector3(
                        t2 * x1 - t1 * x2,
                        t2 * y1 - t1 * y2,
                        t2 * z1 - t1 * z2
                    );
                    var tdir = r * new Vector3(
                        s1 * x2 - s2 * x1,
                        s1 * y2 - s2 * y1,
                        s1 * z2 - s2 * z1
                    );

                    tan1[i1] += sdir;
                    tan1[i2] += sdir;
                    tan1[i3] += sdir;

                    tan2[i1] += tdir;
                    tan2[i2] += tdir;
                    tan2[i3] += tdir;
                }

                for (var a = 0; a < vertices.Length; ++a) {
                    var vert = vertices[a];
                    var n4 = dataSource.GetNormal(vert);
                    var n = new Vector3(n4.X, n4.Y, n4.Z);
                    var t1 = tan1[a];
                    var t2 = tan2[a];

                    var tangent3 = Vector3.Normalize(t1 - n * Vector3.Dot(n, t1));
                    /*vert.Tangent = new Vector4(
                        tangent3,
                        (Vector3.Dot(Vector3.Cross(n, t1), t2) < 0) ? -1 : 1
                    );*/
                    dataSource.SetTangent(ref vert, tangent3);

                    var bin3 = Vector3.Normalize(t2 - n * Vector3.Dot(n, t2));
                    /*vert.Binormal = new Vector4(
                        bin3,
                        (Vector3.Dot(Vector3.Cross(n, t2), t1) < 0) ? -1 : 1
                    );*/
                    dataSource.SetBinormal(ref vert, bin3);

                    vertices[a] = vert;
                }
            }
        }
    }
}
