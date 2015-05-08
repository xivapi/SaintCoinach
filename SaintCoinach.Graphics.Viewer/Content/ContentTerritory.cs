using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer.Content {
    public class ContentTerritory : Drawable3DComponent {
        #region Fields
        private ComponentContainer _TerrainContainer = new ComponentContainer();
        private ComponentContainer _LgbPartsContainer = new ComponentContainer();
        #endregion

        #region Properties
        public Territory Territory { get; private set; }
        #endregion

        #region Constructor
        public ContentTerritory(Engine engine, Territory territory) : base(engine) {
            this.Territory = territory;

            if (territory.Terrain != null) {
                foreach (var part in territory.Terrain.Parts)
                    _TerrainContainer.Add(new ContentModel(engine, part));
            }
            foreach (var lgb in territory.LgbFiles) {
                foreach (var part in lgb.Parts.OfType<TerritoryParts.LgbModelEntry>()) {
                    if (part.Model != null)
                        _TerrainContainer.Add(new ContentModel(engine, part.Model));
                }
            }
        }
        #endregion

        public override void LoadContent() {
            _TerrainContainer.LoadContent();
            _LgbPartsContainer.LoadContent();
            base.LoadContent();
        }
        public override void UnloadContent() {
            _TerrainContainer.UnloadContent();
            _LgbPartsContainer.UnloadContent();
            base.UnloadContent();
        }
        public override void Update(EngineTime engineTime) {
            _TerrainContainer.Update(engineTime);
            _LgbPartsContainer.Update(engineTime);
            base.Update(engineTime);
        }
        public override void Draw(EngineTime time, ref SharpDX.Matrix world, ref SharpDX.Matrix view, ref SharpDX.Matrix projection) {
            _TerrainContainer.Draw(time, ref world, ref view, ref projection);
            _LgbPartsContainer.Draw(time, ref world, ref view, ref projection);
        }
    }
}
