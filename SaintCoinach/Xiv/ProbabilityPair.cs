namespace SaintCoinach.Xiv {
    public class ProbabilityPair<T> {
        #region Properties

        public T Value { get; private set; }
        public double Probability { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public ProbabilityPair(T value, double probability) {
            Value = value;
            Probability = probability;
        }

        #endregion

        #endregion
    }
}
