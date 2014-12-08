using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.ItemActions {
    public class MpRecovery : PointRecovery {
        const int AmountKey = 0;
        const int MaximumKey = 1;

        #region Properties
        public int Amount { get { return GetData(AmountKey); } }
        public int Maximum { get { return GetData(MaximumKey); } }
        public int AmountHq { get { return GetHqData(AmountKey); } }
        public int MaximumHq { get { return GetHqData(MaximumKey); } }
        #endregion

        #region Constructor
        public MpRecovery(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        protected override IEnumerable<Parameter> GetParameters() {
            // XXX: Here be magic numbers
            const int MpBaseParamKey = 8;

            var parameters = new ParameterCollection();
            var bpSheet = Sheet.Collection.GetSheet<BaseParam>();

            if (Maximum > 0)
                parameters.AddParameterValue(bpSheet[MpBaseParamKey], new ParameterValueRelativeLimited(ParameterType.Base, Amount / 100.0, Maximum));
            else
                parameters.AddParameterValue(bpSheet[MpBaseParamKey], new ParameterValueFixed(ParameterType.Base, Amount));

            if (MaximumHq != Maximum && AmountHq != Amount) {
                if (MaximumHq > 0)
                    parameters.AddParameterValue(bpSheet[MpBaseParamKey], new ParameterValueRelativeLimited(ParameterType.HQ, AmountHq / 100.0, MaximumHq));
                else
                    parameters.AddParameterValue(bpSheet[MpBaseParamKey], new ParameterValueFixed(ParameterType.HQ, AmountHq));
            }

            return parameters;
        }
        #endregion
    }
}