using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godbert.Models {
    public class ModelCharaSub : IEnumerable<ModelCharaVariant> {
        #region Properties
        public int Value { get; set; }
        public string DisplayName { get; set; }
        public ModelCharaMain Parent { get; private set; }
        #endregion

        #region Constructor
        public ModelCharaSub(ModelCharaMain parent, string displayName, int value) {
            this.Parent = parent;
            this.Value = value;
            this.DisplayName = displayName;
        }
        #endregion

        #region Fields
        private SortedDictionary<int, ModelCharaVariant> _Items = new SortedDictionary<int, ModelCharaVariant>();
        #endregion

        #region Collection
        public void Add(int variant) {
            if (!_Items.ContainsKey(variant))
                _Items.Add(variant, new ModelCharaVariant(this, string.Format(Parent.Parent.VariantFormat, variant), variant));
        }
        public bool Contains(int variant) {
            return _Items.ContainsKey(variant);
        }
        public void Clear() {
            _Items.Clear();
        }
        #endregion

        #region IEnumerable<ModelCharaMain> Members

        public IEnumerator<ModelCharaVariant> GetEnumerator() {
            return _Items.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        public override string ToString() {
            return string.Format("{0} / {1}", Parent, DisplayName);
        }
    }
}
