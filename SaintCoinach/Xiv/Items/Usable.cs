using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.Items {
    public class Usable : InventoryItem, IParameterObject {
        #region Properties
        public IEnumerable<Parameter> Parameters {
            get {
                var actionAsParamObj = ItemAction as IParameterObject;
                if (actionAsParamObj != null)
                    return actionAsParamObj.Parameters;
                return new Parameter[0];
            }
        }
        public TimeSpan Cooldown { get { return TimeSpan.FromSeconds(AsDouble("Cooldown<s>")); } }
        #endregion

        #region Constructor
        public Usable(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion
    }
}