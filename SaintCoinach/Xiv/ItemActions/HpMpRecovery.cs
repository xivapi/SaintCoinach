using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.ItemActions {
    public class HpMpRecovery : PointRecovery {
        const int HpAmountKey = 0;
        const int HpMaximumKey = 1;

        const int MpAmountKey = 3;
        const int MpMaximumKey = 4;

        #region Properties
        public int HpAmount { get { return GetData(HpAmountKey); } }
        public int HpMaximum { get { return GetData(HpMaximumKey); } }
        public int HpAmountHq { get { return GetHqData(HpAmountKey); } }
        public int HpMaximumHq { get { return GetHqData(HpMaximumKey); } }

        public int MpAmount { get { return GetData(MpAmountKey); } }
        public int MpMaximum { get { return GetData(MpMaximumKey); } }
        public int MpAmountHq { get { return GetHqData(MpAmountKey); } }
        public int MpMaximumHq { get { return GetHqData(MpMaximumKey); } }
        #endregion

        #region Constructor
        public HpMpRecovery(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        protected override IEnumerable<Parameter> GetParameters() {
            // XXX: Here be magic numbers
            const int HpBaseParamKey = 7;
            const int MpBaseParamKey = 8;

            var parameters = new ParameterCollection();
            var bpSheet = Sheet.Collection.GetSheet<BaseParam>();

            if (HpMaximum > 0)
                parameters.AddParameterValue(bpSheet[HpBaseParamKey], new ParameterValueRelativeLimited(ParameterType.Base, HpAmount / 100.0, HpMaximum));
            else
                parameters.AddParameterValue(bpSheet[HpBaseParamKey], new ParameterValueFixed(ParameterType.Base, HpAmount));

            if (HpMaximumHq != HpMaximum && HpAmountHq != HpAmount) {
                if (HpMaximumHq > 0)
                    parameters.AddParameterValue(bpSheet[HpBaseParamKey], new ParameterValueRelativeLimited(ParameterType.HQ, HpAmountHq / 100.0, HpMaximumHq));
                else
                    parameters.AddParameterValue(bpSheet[HpBaseParamKey], new ParameterValueFixed(ParameterType.HQ, HpAmountHq));
            }

            if (MpMaximum > 0)
                parameters.AddParameterValue(bpSheet[MpBaseParamKey], new ParameterValueRelativeLimited(ParameterType.Base, MpAmount / 100.0, MpMaximum));
            else
                parameters.AddParameterValue(bpSheet[MpBaseParamKey], new ParameterValueFixed(ParameterType.Base, MpAmount));

            if (MpMaximumHq != MpMaximum && MpAmountHq != MpAmount) {
                if (MpMaximumHq > 0)
                    parameters.AddParameterValue(bpSheet[MpBaseParamKey], new ParameterValueRelativeLimited(ParameterType.HQ, MpAmountHq / 100.0, MpMaximumHq));
                else
                    parameters.AddParameterValue(bpSheet[MpBaseParamKey], new ParameterValueFixed(ParameterType.HQ, MpAmountHq));
            }

            return parameters;
        }
        #endregion
    }
}