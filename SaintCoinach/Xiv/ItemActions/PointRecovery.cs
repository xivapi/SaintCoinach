using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.ItemActions {
    public abstract class PointRecovery : ItemAction, IParameterObject {
        #region Fields

        private IEnumerable<Parameter> _Parameters;

        #endregion

        #region Constructors

        #region Constructor

        protected PointRecovery(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public IEnumerable<Parameter> Parameters { get { return _Parameters ?? (_Parameters = GetParameters()); } }

        #region Build

        protected abstract IEnumerable<Parameter> GetParameters();

        #endregion
    }
}
