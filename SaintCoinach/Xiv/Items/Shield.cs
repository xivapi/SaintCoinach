using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.Items {
    public class Shield : Equipment {
        #region Fields
        private ParameterCollection _PrimaryParameters = null;
        #endregion

        #region Properties
        public override IEnumerable<Parameter> PrimaryParameters { get { return _PrimaryParameters ?? (_PrimaryParameters = BuildPrimaryParameters()); } }
        public int BlockStrength { get { return AsInt32("Block"); } }
        public int BlockRate { get { return AsInt32("BlockRate"); } }
        #endregion

        #region Constructor
        public Shield(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        private ParameterCollection BuildPrimaryParameters() {
            var param = new ParameterCollection();

            // XXX: Here be magic numbers
            const int BlockStrengthKey = 17;
            const int BlockRateKey = 18;

            var paramSheet = Sheet.Collection.GetSheet<BaseParam>();
            if (BlockStrength != 0)
                param.AddParameterValue(paramSheet[BlockStrengthKey], new ParameterValueFixed(ParameterType.Primary, BlockStrength));
            if (BlockRate != 0)
                param.AddParameterValue(paramSheet[BlockRateKey], new ParameterValueFixed(ParameterType.Primary, BlockRate));

            return param;
        }
        #endregion
    }
}