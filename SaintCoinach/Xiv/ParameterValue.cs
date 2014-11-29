using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public abstract class ParameterValue {
        #region Fields
        private ParameterType _Type;
        #endregion

        #region Properties
        public ParameterType Type { get { return _Type; } }
        #endregion

        #region Constructor
        protected ParameterValue(ParameterType type) {
            _Type = type;
        }
        #endregion
    }
}
