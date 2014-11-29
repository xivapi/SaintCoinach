using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.Items {
    public class Armour : Equipment {
        #region Fields
        private ParameterCollection _PrimaryParameters = null;
        #endregion

        #region Properties
        public override IEnumerable<Parameter> PrimaryParameters { get { return _PrimaryParameters ?? (_PrimaryParameters = BuildPrimaryParameters()); } }
        public int PhysicalDefense { get { return AsInt32("Defense{Phys}"); } }
        public int MagicDefense { get { return AsInt32("Defense{Mag}"); } }
        #endregion

        #region Constructor
        public Armour(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        private ParameterCollection BuildPrimaryParameters() {
            var param = new ParameterCollection();

            // XXX: Here be magic numbers
            const int PhysicalDefenseKey = 21;
            const int MagicDefenseKey = 24;

            var paramSheet = Sheet.Collection.GetSheet<BaseParam>();
            if (PhysicalDefense != 0)
                param.AddParameterValue(paramSheet[PhysicalDefenseKey], new ParameterValueFixed(ParameterType.Primary, PhysicalDefense));
            if (MagicDefense != 0)
                param.AddParameterValue(paramSheet[MagicDefenseKey], new ParameterValueFixed(ParameterType.Primary, MagicDefense));

            return param;
        }
        #endregion
    }
}