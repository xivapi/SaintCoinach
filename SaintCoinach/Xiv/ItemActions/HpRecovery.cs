using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.ItemActions {
    public class HpRecovery : PointRecovery {
        #region Static

        private const int AmountKey = 0;
        private const int MaximumKey = 1;

        #endregion

        #region Properties

        public int Amount { get { return GetData(AmountKey); } }
        public int Maximum { get { return GetData(MaximumKey); } }
        public int AmountHq { get { return GetHqData(AmountKey); } }
        public int MaximumHq { get { return GetHqData(MaximumKey); } }

        #endregion

        #region Constructors

        #region Constructor

        public HpRecovery(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Build

        protected override IEnumerable<Parameter> GetParameters() {
            // XXX: Here be magic numbers
            const int HpBaseParamKey = 7;

            var parameters = new ParameterCollection();
            var bpSheet = Sheet.Collection.GetSheet<BaseParam>();

            if (Maximum > 0)
                parameters.AddParameterValue(bpSheet[HpBaseParamKey],
                    new ParameterValueRelativeLimited(ParameterType.Base, Amount / 100.0, Maximum));
            else
                parameters.AddParameterValue(bpSheet[HpBaseParamKey],
                    new ParameterValueFixed(ParameterType.Base, Amount));

            // ReSharper disable once InvertIf
            if (MaximumHq != Maximum && AmountHq != Amount) {
                if (MaximumHq > 0)
                    parameters.AddParameterValue(bpSheet[HpBaseParamKey],
                        new ParameterValueRelativeLimited(ParameterType.Hq, AmountHq / 100.0, MaximumHq));
                else
                    parameters.AddParameterValue(bpSheet[HpBaseParamKey],
                        new ParameterValueFixed(ParameterType.Hq, AmountHq));
            }

            return parameters;
        }

        #endregion
    }
}
