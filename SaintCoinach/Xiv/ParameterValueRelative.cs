using System.Text;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing a relative parameter bonus.
    /// </summary>
    public class ParameterValueRelative : ParameterValue {
        #region Properties

        /// <summary>
        ///     Gets the current relative bonus.
        /// </summary>
        /// <value>The current relative bonus.</value>
        public double Amount { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParameterValueRelative" /> class.
        /// </summary>
        /// <param name="type"><see cref="ParameterType" /> of the bonus.</param>
        /// <param name="amount">Relative bonus.</param>
        public ParameterValueRelative(ParameterType type, double amount)
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
            sb.Append(Amount.ToString("p0"));

            return sb.ToString();
        }
    }
}
