namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Class representing an object that has a probability attached.
    /// </summary>
    /// <typeparam name="T">Type of the contained object.</typeparam>
    public class ProbabilityPair<T> {
        #region Properties

        /// <summary>
        ///     Gets the object.
        /// </summary>
        /// <value>The object.</value>
        public T Value { get; private set; }

        /// <summary>
        ///     Gets the probability attached to the current object.
        /// </summary>
        /// <value>The probability attached to the current object.</value>
        public double Probability { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProbabilityPair{T}" /> class.
        /// </summary>
        /// <param name="value">Object to attach the probability to.</param>
        /// <param name="probability">Probability to attach to <c>value</c>.</param>
        public ProbabilityPair(T value, double probability) {
            Value = value;
            Probability = probability;
        }

        #endregion
    }
}
