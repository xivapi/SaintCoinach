namespace SaintCoinach.Xiv {
    public class ParameterValueRelativeLimited : ParameterValueRelative {
        #region Properties

        public int Maximum { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public ParameterValueRelativeLimited(ParameterType type, double amount, int maximum)
            : base(type, amount) {
            Maximum = maximum;
        }

        #endregion

        #endregion

        public override string ToString() {
            return base.ToString() + string.Format(" (max. {0})", Maximum);
        }
    }
}
