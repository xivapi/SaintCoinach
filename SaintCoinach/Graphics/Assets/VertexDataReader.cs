using System;

namespace SaintCoinach.Graphics.Assets {
    public abstract class VertexDataReader {
        #region Properties

        public abstract Type VertexType { get; }
        public abstract int Length { get; }

        #endregion

        public abstract void Fill(VertexBase vertex, byte[] data, int offset);
        // 0x0C

        #region NTx

        public static readonly VertexDataReader NormalTexture = new NTxReader();

        private class NTxReader : VertexDataReader {
            #region Properties

            public override Type VertexType { get { return typeof(VertexBase); } }
            public override int Length { get { return 0x0C; } }

            #endregion

            public override void Fill(VertexBase vertex, byte[] data, int offset) {
                const int NormalOffset = 0x00;
                const int TextureOffset = 0x08;

                var nX = HalfHelper.Unpack(data, offset + NormalOffset + 0x00);
                var nY = HalfHelper.Unpack(data, offset + NormalOffset + 0x02);
                var nZ = HalfHelper.Unpack(data, offset + NormalOffset + 0x04);
                vertex.Normal = new Vector3(nX, nY, nZ);

                var tX = HalfHelper.Unpack(data, offset + TextureOffset + 0x00);
                var tY = HalfHelper.Unpack(data, offset + TextureOffset + 0x02);
                vertex.TextureCoordinates0 = new Vector2(tX, tY);
            }
        }

        #endregion

        // 0x10

        #region NTxBlend

        public static readonly VertexDataReader NormalTextureBlend = new NTxBlendReader();

        private class NTxBlendReader : VertexDataReader {
            #region Properties

            public override Type VertexType { get { return typeof(VertexBlend); } }
            public override int Length { get { return 0x10; } }

            #endregion

            public override void Fill(VertexBase vertex, byte[] data, int offset) {
                const int NormalOffset = 0x00;
                const int TextureWeightOffset = 0x08;
                const int TextureOffset = 0x0C;

                var v = (VertexBlend)vertex;

                var nX = HalfHelper.Unpack(data, offset + NormalOffset + 0x00);
                var nY = HalfHelper.Unpack(data, offset + NormalOffset + 0x02);
                var nZ = HalfHelper.Unpack(data, offset + NormalOffset + 0x04);
                v.Normal = new Vector3(nX, nY, nZ);

                v.BlendWeight = new Vector4(
                    1 - data[offset + TextureWeightOffset + 0] / 255f,
                    1 - data[offset + TextureWeightOffset + 1] / 255f,
                    1 - data[offset + TextureWeightOffset + 2] / 255f,
                    1 - data[offset + TextureWeightOffset + 3] / 255f
                    );

                var tX = HalfHelper.Unpack(data, offset + TextureOffset + 0x00);
                var tY = HalfHelper.Unpack(data, offset + TextureOffset + 0x02);
                v.TextureCoordinates0 = new Vector2(tX, tY);
                v.TextureCoordinates1 = vertex.TextureCoordinates0;
            }
        }

        #endregion

        // 0x14

        #region NTxColBlend

        public static readonly VertexDataReader NormalTextureColorBlend = new NTxColBlendReader();

        private class NTxColBlendReader : VertexDataReader {
            #region Properties

            public override Type VertexType { get { return typeof(VertexColorBlend); } }
            public override int Length { get { return 0x14; } }

            #endregion

            public override void Fill(VertexBase vertex, byte[] data, int offset) {
                const int NormalOffset = 0x00;
                const int ColorOffset = 0x0C;
                const int TextureWeightOffset = 0x08;
                const int TextureOffset = 0x10;

                var v = (VertexColorBlend)vertex;

                var nX = HalfHelper.Unpack(data, offset + NormalOffset + 0x00);
                var nY = HalfHelper.Unpack(data, offset + NormalOffset + 0x02);
                var nZ = HalfHelper.Unpack(data, offset + NormalOffset + 0x04);
                vertex.Normal = new Vector3(nX, nY, nZ);

                v.Color0 = new Vector4(
                    data[offset + ColorOffset + 0] / 255f,
                    data[offset + ColorOffset + 1] / 255f,
                    data[offset + ColorOffset + 2] / 255f,
                    data[offset + ColorOffset + 3] / 255f
                    );
                v.Color1 = v.Color0;

                v.BlendWeight = new Vector4(
                    data[offset + TextureWeightOffset + 0] / 255f,
                    data[offset + TextureWeightOffset + 1] / 255f,
                    data[offset + TextureWeightOffset + 2] / 255f,
                    data[offset + TextureWeightOffset + 3] / 255f
                    );

                var tX = HalfHelper.Unpack(data, offset + TextureOffset + 0x00);
                var tY = HalfHelper.Unpack(data, offset + TextureOffset + 0x02);

                v.TextureCoordinates0 = new Vector2(tX, tY);
                v.TextureCoordinates1 = v.TextureCoordinates0;
            }
        }

        #endregion

        // 0x1C

        #region NTx2Col2Blend

        public static readonly VertexDataReader NormalTextureColorBlend2 = new NTx2Col2BlendReader();

        private class NTx2Col2BlendReader : VertexDataReader {
            #region Properties

            public override Type VertexType { get { return typeof(VertexColorBlend); } }
            public override int Length { get { return 0x1C; } }

            #endregion

            public override void Fill(VertexBase vertex, byte[] data, int offset) {
                const int NormalOffset = 0x00;
                const int Color0Offset = 0x08;
                const int Color1Offset = 0x0C;
                const int TextureWeightOffset = 0x10;
                const int TextureOffset = 0x14;

                var v = (VertexColorBlend)vertex;

                var nX = HalfHelper.Unpack(data, offset + NormalOffset + 0x00);
                var nY = HalfHelper.Unpack(data, offset + NormalOffset + 0x02);
                var nZ = HalfHelper.Unpack(data, offset + NormalOffset + 0x04);
                v.Normal = new Vector3(nX, nY, nZ);

                v.Color0 = new Vector4(
                    data[offset + Color0Offset + 0] / 255f,
                    data[offset + Color0Offset + 1] / 255f,
                    data[offset + Color0Offset + 2] / 255f,
                    data[offset + Color0Offset + 3] / 255f
                    );
                v.Color1 = new Vector4(
                    data[offset + Color1Offset + 0] / 255f,
                    data[offset + Color1Offset + 1] / 255f,
                    data[offset + Color1Offset + 2] / 255f,
                    data[offset + Color1Offset + 3] / 255f
                    );

                v.BlendWeight = new Vector4(
                    data[offset + TextureWeightOffset + 0] / 255f,
                    data[offset + TextureWeightOffset + 1] / 255f,
                    data[offset + TextureWeightOffset + 2] / 255f,
                    data[offset + TextureWeightOffset + 3] / 255f
                    );

                {
                    var tX = HalfHelper.Unpack(data, offset + TextureOffset + 0x00);
                    var tY = HalfHelper.Unpack(data, offset + TextureOffset + 0x02);
                    v.TextureCoordinates0 = new Vector2(tX, tY);
                }
                {
                    var tX = HalfHelper.Unpack(data, offset + TextureOffset + 0x04);
                    var tY = HalfHelper.Unpack(data, offset + TextureOffset + 0x06);
                    v.TextureCoordinates1 = new Vector2(tX, tY);
                }
            }
        }

        #endregion

        // 0x18

        #region NTxCol

        public static readonly VertexDataReader NormalTextureColor = new NTxColReader();

        private class NTxColReader : VertexDataReader {
            #region Properties

            public override Type VertexType { get { return typeof(VertexColor); } }
            public override int Length { get { return 0x18; } }

            #endregion

            public override void Fill(VertexBase vertex, byte[] data, int offset) {
                const int NormalOffset = 0x00;
                const int ColorOffset = 0x0C;
                const int TextureOffset = 0x10;

                var v = (VertexColor)vertex;

                var nX = HalfHelper.Unpack(data, offset + NormalOffset + 0x00);
                var nY = HalfHelper.Unpack(data, offset + NormalOffset + 0x02);
                var nZ = HalfHelper.Unpack(data, offset + NormalOffset + 0x04);
                v.Normal = new Vector3(nX, nY, nZ);

                var cX = data[offset + ColorOffset + 0] / 255f;
                var cY = data[offset + ColorOffset + 1] / 255f;
                var cZ = data[offset + ColorOffset + 2] / 255f;
                var cW = data[offset + ColorOffset + 3] / 255f;
                v.Color0 = new Vector4(cX, cY, cZ, cW);

                var tX = HalfHelper.Unpack(data, offset + TextureOffset + 0x00);
                var tY = HalfHelper.Unpack(data, offset + TextureOffset + 0x02);
                v.TextureCoordinates0 = new Vector2(tX, tY);
            }
        }

        #endregion
    }
}
