using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class ItemFood : XivRow, IParameterObject {
        #region Fields

        private ParameterCollection _Parameters;

        #endregion

        #region Constructors

        #region Constructor

        public ItemFood(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public IEnumerable<Parameter> Parameters { get { return _Parameters ?? (_Parameters = BuildParameters()); } }

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

                    parameters.AddParameterValue(param,
                        max == 0
                            ? new ParameterValueRelative(ParameterType.Base, val / 100.0)
                            : new ParameterValueRelativeLimited(ParameterType.Base, val / 100.0, max));

                    if (maxHq == max && valHq == val) continue;

                    parameters.AddParameterValue(param,
                        maxHq == 0
                            ? new ParameterValueRelative(ParameterType.Hq, valHq / 100.0)
                            : new ParameterValueRelativeLimited(ParameterType.Hq, valHq / 100.0, maxHq));
                } else {
                    parameters.AddParameterValue(param, new ParameterValueFixed(ParameterType.Base, val));
                    if (val != valHq)
                        parameters.AddParameterValue(param, new ParameterValueFixed(ParameterType.Hq, valHq));
                }
            }
            return parameters;
        }

        #endregion
    }
}
