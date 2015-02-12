using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.ItemActions {
    public class TpRecovery : PointRecovery {
        #region Static

        private const int AmountKey = 0;

        #endregion

        #region Properties

        public int Amount { get { return GetData(AmountKey); } }
        public int AmountHq { get { return GetHqData(AmountKey); } }

        #endregion

        #region Constructors

        #region Constructor

        public TpRecovery(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Build

        protected override IEnumerable<Parameter> GetParameters() {
            const int TpBaseParamKey = 9; // XXX: Magic number!

            var parameters = new ParameterCollection();

            var bpSheet = Sheet.Collection.GetSheet<BaseParam>();
            parameters.AddParameterValue(bpSheet[TpBaseParamKey], new ParameterValueFixed(ParameterType.Base, Amount));
            if (AmountHq != Amount)
                parameters.AddParameterValue(bpSheet[TpBaseParamKey],
                    new ParameterValueFixed(ParameterType.Hq, AmountHq));

            return parameters;
        }

        #endregion
    }
}
