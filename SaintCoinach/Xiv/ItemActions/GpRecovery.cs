using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.ItemActions {
    public class GpRecovery : PointRecovery {
        const int AmountKey = 0;

        #region Properties
        public int Amount { get { return GetData(AmountKey); } }
        public int AmountHq { get { return GetHqData(AmountKey); } }
        #endregion

        #region Constructor
        public GpRecovery(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        protected override IEnumerable<Parameter> GetParameters() {
            const int GpBaseParamKey = 10;  // XXX: Magic number!

            var parameters = new ParameterCollection();

            var bpSheet = Sheet.Collection.GetSheet<BaseParam>();
            parameters.AddParameterValue(bpSheet[GpBaseParamKey], new ParameterValueFixed(ParameterType.Base, Amount));
            if (AmountHq != Amount)
                parameters.AddParameterValue(bpSheet[GpBaseParamKey], new ParameterValueFixed(ParameterType.HQ, AmountHq));

            return parameters;
        }
        #endregion
    }
}