using System.Collections.Generic;

namespace SaintCoinach.Xiv {
    public interface IParameterObject {
        #region Properties

        IEnumerable<Parameter> Parameters { get; }

        #endregion
    }
}
