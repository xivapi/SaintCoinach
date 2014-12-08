using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ItemFood : XivRow, IParameterObject {
        #region Fields
        private ParameterCollection _Parameters;
        #endregion

        #region Properties
        public IEnumerable<Parameter> Parameters { get { return _Parameters ?? (_Parameters = BuildParameters()); } }
        #endregion

        #region Constructor
        public ItemFood(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        private ParameterCollection BuildParameters() {
            const int Count = 3;

            var parameters = new ParameterCollection();
            for (var i = 0; i < Count; ++i) {
                var param = As<BaseParam>(i);
                if (param.Key == 0)
                    continue;

                var isRel = AsBoolean("IsRelative", i);
                var val = AsInt32("Value", i);
                var valHq = AsInt32("Value{HQ}", i);

                if (isRel) {
                    var max = AsInt32("Max", i);
                    var maxHq = AsInt32("Max{HQ}", i);

                    if (max == 0)
                        parameters.AddParameterValue(param, new ParameterValueRelative(ParameterType.Base, val / 100.0));
                    else
                        parameters.AddParameterValue(param, new ParameterValueRelativeLimited(ParameterType.Base, val / 100.0, max));

                    if (maxHq != max || valHq != val) {
                        if (maxHq == 0)
                            parameters.AddParameterValue(param, new ParameterValueRelative(ParameterType.HQ, valHq / 100.0));
                        else
                            parameters.AddParameterValue(param, new ParameterValueRelativeLimited(ParameterType.HQ, valHq / 100.0, maxHq));
                    }
                } else {
                    parameters.AddParameterValue(param, new ParameterValueFixed(ParameterType.Base, val));
                    if (val != valHq)
                        parameters.AddParameterValue(param, new ParameterValueFixed(ParameterType.HQ, valHq));
                }
            }
            return parameters;
        }
        #endregion
    }
}