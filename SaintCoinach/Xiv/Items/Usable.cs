using System;
using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.Items {
    public class Usable : Item, IParameterObject {
        #region Properties

        public TimeSpan Cooldown { get { return TimeSpan.FromSeconds(AsDouble("Cooldown<s>")); } }

        #endregion

        #region Constructors

        #region Constructor

        public Usable(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public IEnumerable<Parameter> Parameters {
            get {
                var actionAsParamObj = ItemAction as IParameterObject;
                return actionAsParamObj != null ? actionAsParamObj.Parameters : new Parameter[0];
            }
        }
    }
}
