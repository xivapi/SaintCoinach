using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ParameterValueRelative : ParameterValue {
        #region Fields
        private double _Amount;
        #endregion

        #region Properties
        public double Amount { get { return _Amount; } }
        #endregion

        #region Constructor
        public ParameterValueRelative(ParameterType type, double amount)
            : base(type) {
            _Amount = amount;
        }
        #endregion

        public override string ToString() {
            var sb = new StringBuilder();

            if (Amount > 0 && Type != ParameterType.Primary && Type != ParameterType.Base)
                sb.Append('+');
            sb.Append(Amount.ToString("p0"));

            return sb.ToString();
        }
    }
}
