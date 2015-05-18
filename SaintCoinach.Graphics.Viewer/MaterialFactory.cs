using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    public class MaterialFactory : IDisposable {
        #region Fields
        private Engine _Engine;
        private Dictionary<string, Content.MaterialBase> _Materials = new Dictionary<string, Content.MaterialBase>();
        #endregion

        #region Constructor
        public MaterialFactory(Engine engine) {
            _Engine = engine;
        }
        #endregion

        #region Get
        public Content.MaterialBase Get(Material material) {
            var key = material.File.Path;

            Content.MaterialBase content;
            if (_Materials.TryGetValue(key, out content))
                return content;

            switch (material.Shader) {
                case "character.shpk":
                    content = new Content.CharacterMaterial(_Engine, material);
                    break;
                case "hair.shpk":
                    content = new Content.HairMaterial(_Engine, material);
                    break;
                case "skin.shpk":
                    content = new Content.SkinMaterial(_Engine, material);
                    break;
                case "bg.shpk":
                case "bguvscroll.shpk": // TODO: Actually make it UV-scroll
                    content = new Content.BgMaterial(_Engine, material);
                    break;
                case "iris.shpk":
                    content = new Content.IrisMaterial(_Engine, material);
                    break;
                case "bgcolorchange.shpk":
                    content = new Content.BgColorChangeMaterial(_Engine, material);
                    break;
                case "crystal.shpk":
                    content = new Content.CrystalMaterial(_Engine, material);
                    break;
                default:
                    throw new NotSupportedException(string.Format("Shader type {0} is not supported.", material.Shader));
            }

            _Materials.Add(key, content);
            return content;
        }
        #endregion

        #region IDisposable Members

        public void Dispose() {
            UnloadAll();
        }

        public void UnloadAll() {
            _Materials.Clear();
        }
        #endregion
    }
}
