using System.Collections.Generic;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv.ItemActions {
    public class GpRecovery : PointRecovery {
        #region Static

        private const int AmountKey = 0;

        #endregion

        #region Properties

        public int Amount { get { return GetData(AmountKey); } }
        public int AmountHq { get { return GetHqData(AmountKey); } }

        #endregion

        #region Constructors

        #region Constructor

        public GpRecovery(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        #region Build

        protected override IEnumerable<Parameter> GetParameters() {
            const int GpBaseParamKey = 10; // XXX: Magic number!

            var parameters = new ParameterCollection();

            var bpSheet = Sheet.Collection.GetSheet<BaseParam>();
            parameters.AddParameterValue(bpSheet[GpBaseParamKey], new ParameterValueFixed(ParameterType.Base, Amount));
            if (AmountHq != Amount)
                parameters.AddParameterValue(bpSheet[GpBaseParamKey],
                    new ParameterValueFixed(ParameterType.Hq, AmountHq));

            return parameters;
        }

        #endregion
    }
}
