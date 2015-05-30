using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Data {
    using SharpDX;

    public class CustomizeParameters : ParametersBase {
        public static readonly CustomizeParameters Default = new CustomizeParameters();

        public const string CustomizeParametersKey = "CustomizeParameters";

        public const string SkinColorKey = "SkinColor";
        public const string LipColorKey = "LipColor";
        public const string HairColorKey = "HairColor";
        public const string MeshColorKey = "MeshColor";
        public const string LeftEyeColorKey = "LeftEyeColor";
        public const string RightEyeColorKey = "RightEyeColor";

        public static Vector4 DefaultSkinColor = new Vector4(210 / 255f, 180 / 255f, 140 / 255f, 1);
        public static Vector4 DefaultLipColor = new Vector4(1, 0, 0, .25f);
        public static Vector3 DefaultHairColor = new Vector3(165 / 255f, 42 / 255f, 42 / 255f);
        public static Vector3 DefaultMeshColor = new Vector3(121 / 255f, 68 / 255f, 59 / 255f);
        public static Vector3 DefaultLeftEyeColor = new Vector3(165 / 255f, 42 / 255f, 42 / 255f);
        public static Vector3 DefaultRightEyeColor = new Vector3(121 / 255f, 68 / 255f, 59 / 255f);

        public Vector4 SkinColor {
            get { return this.GetValueOrDefault(SkinColorKey, DefaultSkinColor); }
            set { Set(SkinColorKey, value); }
        }
        public Vector4 LipColor {
            get { return this.GetValueOrDefault(LipColorKey, DefaultLipColor); }
            set { Set(LipColorKey, value); }
        }
        public Vector3 HairColor {
            get { return this.GetValueOrDefault(HairColorKey, DefaultHairColor); }
            set { Set(HairColorKey, value); }
        }
        public Vector3 MeshColor {
            get { return this.GetValueOrDefault(MeshColorKey, DefaultMeshColor); }
            set { Set(MeshColorKey, value); }
        }
        public Vector3 LeftEyeColor {
            get { return this.GetValueOrDefault(LeftEyeColorKey, DefaultLeftEyeColor); }
            set { Set(LeftEyeColorKey, value); }
        }
        public Vector3 RightEyeColor {
            get { return this.GetValueOrDefault(RightEyeColorKey, DefaultRightEyeColor); }
            set { Set(RightEyeColorKey, value); }
        }

        #region Constructor
        public CustomizeParameters() { Set(CustomizeParametersKey, this); }
        public CustomizeParameters(ParametersBase copyFrom) : base(copyFrom) {
            Set(CustomizeParametersKey, this);
        }
        #endregion

        public override object Clone() {
            return new CustomizeParameters(this);
        }
    }
}
