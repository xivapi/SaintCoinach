using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    using SharpDX;
    using SharpDX.Direct3D11;
    using SharpDX.DXGI;

    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex3D {
        public Vector4 Position;
        public Vector3 Normal;
        public Vector4 UV;
        public Vector4 BlendWeights;
        public uint BlendIndices;
        public Vector4 Color;
        public Vector4 Tangent1;
        public Vector4 Tangent2;

        public static InputElement[] InputElements {
            get {
                return new[] {
                    new InputElement("SV_Position", 0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 0),
                    new InputElement("NORMAL", 0, Format.R32G32B32_Float, InputElement.AppendAligned, 0),
                    new InputElement("TEXCOORD", 0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 0),
                    new InputElement("BLENDWEIGHT", 0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 0),
                    new InputElement("BLENDINDICES", 0, Format.R8G8B8A8_UInt, InputElement.AppendAligned, 0),
                    new InputElement("COLOR", 0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 0),
                    new InputElement("TANGENT", 0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 0),
                    new InputElement("TANGENT", 1, Format.R32G32B32A32_Float, InputElement.AppendAligned, 0),
                };
            }
        }

        public Vertex3D(Vertex src) {
            this.Position = src.Position.ToDx(Vector4.Zero);
            this.Position.W = 1;
            this.Normal = src.Normal.ToDx(Vector3.Zero);
            this.UV = src.UV.ToDx(Vector4.Zero);
            this.BlendWeights = src.BlendWeights.ToDx(Vector4.Zero);
            this.BlendIndices = src.BlendIndices.GetValueOrDefault(0);
            this.Color = src.Color.ToDx(Vector4.One);
            this.Tangent2 = src.Tangent2.ToDx(Vector4.Zero);
            this.Tangent1 = src.Tangent1.ToDx(Vector4.Zero);
        }
    }
}
