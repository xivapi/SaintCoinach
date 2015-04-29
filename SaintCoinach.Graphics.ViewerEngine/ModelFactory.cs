using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class ModelFactory {
        #region Fields
        private Dictionary<string, Model> _Cache = new Dictionary<string, Model>();
        #endregion

        #region Things
        public Model GetModel(Assets.ModelFile file) {
            Model model;
            var key = file.Path;
            if (_Cache.TryGetValue(key, out model))
                return model;

            var sub = file.GetModel().GetSubModel(0);
            model = new Model(sub);
            _Cache.Add(key, model);
            return model;
        }
        #endregion
    }
}
