using System.Text;

namespace SaintCoinach.Xiv {
    public class ParameterValueFixed : ParameterValue {
        #region Properties

        public double Amount { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public ParameterValueFixed(ParameterType type, double amount)
            : base(type) {
            Amount = amount;
        }

        #endregion

        #endregion

        public override string ToString() {
            var sb = new StringBuilder();

            if (Amount > 0 && Type != ParameterType.Primary && Type != ParameterType.Base)
                sb.Append('+');
            sb.Append(Amount);

            return sb.ToString();
        }
    }
}
