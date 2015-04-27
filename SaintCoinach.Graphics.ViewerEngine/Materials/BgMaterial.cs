using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D11;

namespace SaintCoinach.Graphics.Materials {
    public class BgMaterial : XivMaterial {
        #region Fields
        private ShaderResourceView _Diffuse0;
        private ShaderResourceView _Specular0;
        private ShaderResourceView _Normal0;
        private ShaderResourceView _Diffuse1;
        private ShaderResourceView _Specular1;
        private ShaderResourceView _Normal1;
        #endregion

        #region Properties
        public ShaderResourceView Diffuse0 { get { return _Diffuse0; } }
        public ShaderResourceView Specular0 { get { return _Specular0; } }
        public ShaderResourceView Normal0 { get { return _Normal0; } }
        public ShaderResourceView Diffuse1 { get { return _Diffuse1; } }
        public ShaderResourceView Specular1 { get { return _Specular1; } }
        public ShaderResourceView Normal1 { get { return _Normal1; } }

        public new Effects.BgEffect Effect { get { return (Effects.BgEffect)base.Effect; } }
        #endregion

        #region Constructor
        public BgMaterial(Device device, Assets.MaterialVersion material)
            : base(device, material, EffectCache.Get(device).BgEffect) {

            var texCache = TextureCache.Get(device);
            foreach (var texParam in material.ParameterMappings) {
                var tex = material.Textures[texParam.Index];

                string target;
                if (!Utilities.Effect.BgParameterMap.TryGetValue(texParam.Id, out target)) {
                    System.Diagnostics.Trace.WriteLine(string.Format("Failed to map parameter with id {0:X8}h (on {1})", texParam.Id, tex.Path));
                    continue;
                }

                switch (target) {
                    case "Diffuse0":
                        _Diffuse0 = texCache.GetResource(tex);
                        break;
                    case "Specular0":
                        _Specular0 = texCache.GetResource(tex);
                        break;
                    case "Normal0":
                        _Normal0 = texCache.GetResource(tex);
                        break;
                    case "Diffuse1":
                        if (tex.Path.Contains("/dummy"))
                            _Diffuse1 = _Diffuse0;
                        else
                            _Diffuse1 = texCache.GetResource(tex);
                        break;
                    case "Specular1":
                        if (tex.Path.Contains("/dummy"))
                            _Specular1 = _Specular0;
                        else
                            _Specular1 = texCache.GetResource(tex);
                        break;
                    case "Normal1":
                        if (tex.Path.Contains("/dummy"))
                            _Normal1 = _Normal0;
                        else
                            _Normal1 = texCache.GetResource(tex);
                        break;
                    default:
                        System.Diagnostics.Trace.WriteLine(string.Format("Unknown parameter {0} for {1}.", texParam.Id, tex.Path));
                        break;
                }
            }

            if (_Normal1 == null)
                _Normal1 = _Normal0;
            if (_Diffuse1 == null)
                _Diffuse1 = _Diffuse0;

            CurrentTechniqueName = Utilities.Effect.GetTechniqueName(material);
        }
        #endregion

        #region Apply
        public override void Apply() {
            Effect.Diffuse0 = this.Diffuse0;
            Effect.Specular0 = this.Specular0;
            Effect.Normal0 = this.Normal0;
            Effect.Diffuse1 = this.Diffuse1;
            Effect.Specular1 = this.Specular1;
            Effect.Normal1 = this.Normal1;
        }
        #endregion
    }
}
