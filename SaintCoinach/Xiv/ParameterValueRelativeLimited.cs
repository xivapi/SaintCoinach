namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing a relative parameter bonus that can only grant up to a certain amount.
    /// </summary>
    public class ParameterValueRelativeLimited : ParameterValueRelative {
        #region Properties

        /// <summary>
        ///     Gets the maximum bonus granted.
        /// </summary>
        /// <value>The maximum bonus granted.</value>
        public int Maximum { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParameterValueRelativeLimited" /> class.
        /// </summary>
        /// <param name="type"><see cref="ParameterType" /> of the bonus.</param>
        /// <param name="amount">Relative bonus.</param>
        /// <param name="maximum">Maximum bonus granted.</param>
        public ParameterValueRelativeLimited(ParameterType type, double amount, int maximum)
            : base(type, amount) {
            Maximum = maximum;
        }

        #endregion

        /// <summary>
        ///     Returns a string that represents the current bonus.
        /// </summary>
        /// <returns>A string that represents the current bonus.</returns>
        public override string ToString() {
            return base.ToString() + string.Format(" (max. {0})", Maximum);
        }
    }
}
