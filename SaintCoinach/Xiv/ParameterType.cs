namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Enumeration of known types of parameter bonuses.
    /// </summary>
    public enum ParameterType {
        /// <summary>
        ///     Value indicating the bonus is always present on an object.
        /// </summary>
        Base,

        /// <summary>
        ///     Value indicating the bonus is a primary parameter and always present on an object.
        /// </summary>
        Primary,

        /// <summary>
        ///     Value indicating the bonus is only valid when set conditions are fulfilled.
        /// </summary>
        SetBonus,

        /// <summary>
        ///     Value indicating the bonus is only valid while a Grand Company sanction is active.
        /// </summary>
        Sanction,

        /// <summary>
        ///     Value indicating the bonus is only valid when an object is high-quality.
        /// </summary>
        Hq,

        /// <summary>
        ///     Value indicating the bonus is variable and can differ between instance.
        /// </summary>
        Variable
    }
}
