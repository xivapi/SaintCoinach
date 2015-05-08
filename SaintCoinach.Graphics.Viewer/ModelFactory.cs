using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics.Viewer {
    public class ModelFactory : IDisposable {
        #region Fields
        private Engine _Engine;
        private Dictionary<Tuple<string, ModelQuality>, Content.PrimitiveModel> _Primitives = new Dictionary<Tuple<string, ModelQuality>, Content.PrimitiveModel>();
        #endregion

        #region Constructor
        public ModelFactory(Engine engine) {
            _Engine = engine;
        }
        #endregion

        #region Get
        public Content.PrimitiveModel Get(ModelDefinition definition, ModelQuality quality) {
            var key = Tuple.Create(definition.File.Path, quality);

            Content.PrimitiveModel primitive;
            if (_Primitives.TryGetValue(key, out primitive))
                return primitive;

            var mdl = definition.GetModel(quality);

            primitive = new Content.PrimitiveModel(_Engine.Device, mdl);
            _Primitives.Add(key, primitive);

            return primitive;
        }
        #endregion

        #region IDisposable Members

        public void Dispose() {
            UnloadAll();
        }

        public void UnloadAll() {
            foreach (var p in _Primitives.Values)
                p.Dispose();
            _Primitives.Clear();
        }

        #endregion
    }
}
