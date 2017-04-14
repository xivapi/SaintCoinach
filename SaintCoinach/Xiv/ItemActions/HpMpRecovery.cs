using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.ItemActions {
    public class HpMpRecovery : PointRecovery {
        #region Static

        private const int HpAmountKey = 0;
        private const int HpMaximumKey = 1;
        private const int MpAmountKey = 3;
        private const int MpMaximumKey = 4;

        #endregion

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

        #region Constructors

        #region Constructor

        public HpMpRecovery(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Build

        protected override IEnumerable<Parameter> GetParameters() {
            // XXX: Here be magic numbers
            const int HpBaseParamKey = 7;
            const int MpBaseParamKey = 8;

            var parameters = new ParameterCollection();
            var bpSheet = Sheet.Collection.GetSheet<BaseParam>();

            if (HpMaximum > 0)
                parameters.AddParameterValue(bpSheet[HpBaseParamKey],
                    new ParameterValueRelativeLimited(ParameterType.Base, HpAmount / 100.0, HpMaximum, 0));
            else
                parameters.AddParameterValue(bpSheet[HpBaseParamKey],
                    new ParameterValueFixed(ParameterType.Base, HpAmount, 0));

            if (HpMaximumHq != HpMaximum && HpAmountHq != HpAmount) {
                if (HpMaximumHq > 0)
                    parameters.AddParameterValue(bpSheet[HpBaseParamKey],
                        new ParameterValueRelativeLimited(ParameterType.Hq, HpAmountHq / 100.0, HpMaximumHq, 0));
                else
                    parameters.AddParameterValue(bpSheet[HpBaseParamKey],
                        new ParameterValueFixed(ParameterType.Hq, HpAmountHq, 0));
            }

            if (MpMaximum > 0)
                parameters.AddParameterValue(bpSheet[MpBaseParamKey],
                    new ParameterValueRelativeLimited(ParameterType.Base, MpAmount / 100.0, MpMaximum, 0));
            else
                parameters.AddParameterValue(bpSheet[MpBaseParamKey],
                    new ParameterValueFixed(ParameterType.Base, MpAmount, 0));

            // ReSharper disable once InvertIf
            if (MpMaximumHq != MpMaximum && MpAmountHq != MpAmount) {
                if (MpMaximumHq > 0)
                    parameters.AddParameterValue(bpSheet[MpBaseParamKey],
                        new ParameterValueRelativeLimited(ParameterType.Hq, MpAmountHq / 100.0, MpMaximumHq, 0));
                else
                    parameters.AddParameterValue(bpSheet[MpBaseParamKey],
                        new ParameterValueFixed(ParameterType.Hq, MpAmountHq, 0));
            }

            return parameters;
        }

        #endregion
    }
}
