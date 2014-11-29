using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ParameterValueRelativeLimited : ParameterValueRelative {
        #region Fields
        private int _Maximum;
        #endregion

        #region Properties
        public int Maximum { get { return _Maximum; } }
        #endregion

        #region Constructor
        public ParameterValueRelativeLimited(ParameterType type, double amount, int maximum)
            : base(type, amount) {
            _Maximum = maximum;
        }
        #endregion

        public override string ToString() {
            return base.ToString() + string.Format(" (max. {0})", Maximum);
        }
    }
}
