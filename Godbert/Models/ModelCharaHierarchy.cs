using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Xiv;

namespace Godbert.Models {
    public class ModelCharaHierarchy : IEnumerable<ModelCharaMain> {
        #region Properties
        public string MainFormat { get; set; }
        public string SubFormat { get; set; }
        public string VariantFormat { get; set; }
        #endregion

        #region Constructor
        public ModelCharaHierarchy(string mainFormat, string subFormat, string variantFormat) {
            this.MainFormat = mainFormat;
            this.SubFormat = subFormat;
            this.VariantFormat = variantFormat;
        }
        #endregion

        #region Fields
        private SortedDictionary<int, ModelCharaMain> _Items = new SortedDictionary<int, ModelCharaMain>();
        #endregion

        #region Collection
        public void Add(ModelChara modelChara) {
            Add(modelChara.ModelKey, modelChara.BaseKey, modelChara.Variant);
        }
        public void Add(int main, int sub, int variant) {
            ModelCharaMain mcm;
            if (!_Items.TryGetValue(main, out mcm))
                _Items.Add(main, mcm = new ModelCharaMain(this, string.Format(MainFormat, main), main));
            mcm.Add(sub, variant);
        }
        public bool Contains(int main, int sub, int variant) {
            ModelCharaMain mcm;
            if (!_Items.TryGetValue(main, out mcm))
                return false;
            return mcm.Contains(sub, variant);
        }
        public void Clear() {
            _Items.Clear();
        }
        #endregion

        #region IEnumerable<ModelCharaMain> Members

        public IEnumerator<ModelCharaMain> GetEnumerator() {
            return _Items.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
