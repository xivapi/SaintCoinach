namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Base class for representing a parameter bonus.
    /// </summary>
    public abstract class ParameterValue {
        #region Properties

        /// <summary>
        ///     Gets the <see cref="ParameterType" /> of the current bonus.
        /// </summary>
        /// <value>The <see cref="ParameterType" /> of the current bonus.</value>
        public ParameterType Type { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParameterValue" /> class.
        /// </summary>
        /// <param name="type"><see cref="ParameterType" /> of the bonus.</param>
        protected ParameterValue(ParameterType type) {
            Type = type;
        }

        #endregion
    }
}
