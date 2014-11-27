using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D11;

namespace SaintCoinach.Graphics.Materials {
    public class CharacterMaterial : XivMaterial {
        #region Fields
        private Texture2D _Diffuse;
        private Texture2D _Specular;
        private Texture2D _Normal;
        private Texture2D _Mask;
        private Texture2D _Table;
        #endregion

        #region Properties
        public Texture2D Diffuse { get { return _Diffuse; } }
        public Texture2D Specular { get { return _Specular; } }
        public Texture2D Normal { get { return _Normal; } }
        public Texture2D Mask { get { return _Mask; } }
        public Texture2D Table { get { return _Table; } }

        public new Effects.CharacterEffect Effect { get { return (Effects.CharacterEffect)base.Effect; } }
        #endregion

        #region Constructor
        public CharacterMaterial(Device device, Assets.MaterialVersion material)
            : base(device, material, EffectCache.Get(device).CharacterEffect) {


            var texCache = TextureCache.Get(device);
            foreach (var texParam in material.ParameterMappings) {
                var tex = material.Textures[texParam.Index];

                string target;
                if (!Utilities.Effect.CharacterParameterMap.TryGetValue(texParam.Id, out target)) {
                    System.Diagnostics.Trace.WriteLine(string.Format("Failed to map parameter with id {0:X8}h (on {1})", texParam.Id, tex.Path));
                    continue;
                }

                switch (target) {
                    case "Diffuse":
                        _Diffuse = texCache.Get(tex);
                        break;
                    case "Specular":
                        _Specular = texCache.Get(tex);
                        break;
                    case "Normal":
                        _Normal = texCache.Get(tex);
                        break;
                    case "Mask":
                        _Mask = texCache.Get(tex);
                        break;
                    case "Table":
                        _Table = texCache.Get(tex);
                        break;
                    default:
                        System.Diagnostics.Trace.WriteLine(string.Format("Unknown parameter {0} for {1}.", texParam.Id, tex.Path));
                        break;
                }
            }

            CurrentTechniqueName = Utilities.Effect.GetTechniqueName(material);
        }
        #endregion

        #region Apply
        public override void Apply() {
            Effect.Diffuse = this.Diffuse;
            Effect.Specular = this.Specular;
            Effect.Normal = this.Normal;
            Effect.Mask = this.Mask;
            Effect.Table = this.Table;
        }
        #endregion
    }
}
