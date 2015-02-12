using System.Collections.Generic;

namespace SaintCoinach.Xiv {
    /// <summary>
    ///     Interface for objects that offer parameter bonuses.
    /// </summary>
    public interface IParameterObject {
        #region Properties

        /// <summary>
        ///     Gets the parameters offered by the current object.
        /// </summary>
        /// <value>The parameters offered by the current object.</value>
        IEnumerable<Parameter> Parameters { get; }

        #endregion
    }
}
