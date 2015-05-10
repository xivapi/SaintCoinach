using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godbert.Models {
    public class ModelCharaVariant {
        #region Properties
        public int Value { get; set; }
        public string DisplayName { get; set; }
        public ModelCharaSub Parent { get; private set; }
        #endregion

        #region Constructor
        public ModelCharaVariant(ModelCharaSub parent, string displayName, int value) {
            this.Parent = parent;
            this.Value = value;
            this.DisplayName = displayName;
        }
        #endregion

        public override string ToString() {
            return string.Format("{0} / {1}", Parent, DisplayName);
        }
    }
}
