using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godbert.Models {
    public class ModelCharaMain : IEnumerable<ModelCharaSub> {
        #region Properties
        public int Value { get; set; }
        public string DisplayName { get; set; }
        public ModelCharaHierarchy Parent { get; private set; }
        #endregion

        #region Constructor
        public ModelCharaMain(ModelCharaHierarchy parent, string displayName, int value) {
            this.Parent = parent;
            this.Value = value;
            this.DisplayName = displayName;
        }
        #endregion

        #region Fields
        private SortedDictionary<int, ModelCharaSub> _Items = new SortedDictionary<int, ModelCharaSub>();
        #endregion

        #region Collection
        public void Add(int sub, int variant) {
            ModelCharaSub mcs;
            if (!_Items.TryGetValue(sub, out mcs))
                _Items.Add(sub, mcs = new ModelCharaSub(this, string.Format(Parent.SubFormat, sub), sub));
            mcs.Add(variant);
        }
        public bool Contains(int sub, int variant) {
            ModelCharaSub mcs;
            if (!_Items.TryGetValue(sub, out mcs))
                return false;
            return mcs.Contains(variant);
        }
        public void Clear() {
            _Items.Clear();
        }
        #endregion

        #region IEnumerable<ModelCharaMain> Members

        public IEnumerator<ModelCharaSub> GetEnumerator() {
            return _Items.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        public override string ToString() {
            return DisplayName;
        }
    }
}
