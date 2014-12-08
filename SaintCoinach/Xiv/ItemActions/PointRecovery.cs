using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.ItemActions {
    public abstract class PointRecovery : ItemAction, IParameterObject {
        #region Fields
        private IEnumerable<Parameter> _Parameters = null;
        #endregion

        #region Properties
        public IEnumerable<Parameter> Parameters { get { return _Parameters ?? (_Parameters = GetParameters()); } }
        #endregion

        #region Constructor
        public PointRecovery(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        protected abstract IEnumerable<Parameter> GetParameters();
        #endregion
    }
}