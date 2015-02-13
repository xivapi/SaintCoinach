using System.Text;

namespace SaintCoinach.Xiv {
    /// <summary>
    /// Class representing a fixed-value parameter bonus.
    /// </summary>
    public class ParameterValueFixed : ParameterValue {
        #region Properties

        /// <summary>
        /// Gets the current bonus amount.
        /// </summary>
        /// <value>The current bonus amount.</value>
        public double Amount { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterValueFixed"/> class.
        /// </summary>
        /// <param name="type"><see cref="ParameterType"/> of the bonus.</param>
        /// <param name="amount">Bonus amount.</param>
        public ParameterValueFixed(ParameterType type, double amount)
            : base(type) {
            Amount = amount;
        }

        #endregion

        /// <summary>
        ///     Returns a string that represents the current bonus.
        /// </summary>
        /// <returns>A string that represents the current bonus.</returns>
        public override string ToString() {
            var sb = new StringBuilder();

            if (Amount > 0 && Type != ParameterType.Primary && Type != ParameterType.Base)
                sb.Append('+');
            sb.Append(Amount);

            return sb.ToString();
        }
    }
}
