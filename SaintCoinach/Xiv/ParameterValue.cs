namespace SaintCoinach.Xiv {
    public abstract class ParameterValue {
        #region Properties

        public ParameterType Type { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        protected ParameterValue(ParameterType type) {
            Type = type;
        }

        #endregion

        #endregion
    }
}
