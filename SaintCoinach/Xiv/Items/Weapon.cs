using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.Items {
    public class Weapon : Equipment {
        #region Fields
        private ParameterCollection _PrimaryParameters = null;
        #endregion

        #region Properties
        public override IEnumerable<Parameter> PrimaryParameters { get { return _PrimaryParameters ?? (_PrimaryParameters = BuildPrimaryParameters()); } }
        public int PhysicalDamage { get { return AsInt32("Damage{Phys}"); } }
        public int MagicDamage { get { return AsInt32("Damage{Mag}"); } }
        public TimeSpan Delay { get { return TimeSpan.FromMilliseconds(AsDouble("Delay<ms>")); } }
        #endregion

        #region Constructor
        public Weapon(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        #region Build
        private ParameterCollection BuildPrimaryParameters() {
            var param = new ParameterCollection();

            // XXX: Here be magic numbers
            const int PhysicalDamageKey = 12;
            const int MagicDamageKey = 13;
            const int DelayKey = 14;

            var paramSheet = Sheet.Collection.GetSheet<BaseParam>();
            if (PhysicalDamage != 0)
                param.AddParameterValue(paramSheet[PhysicalDamageKey], new ParameterValueFixed(ParameterType.Primary, PhysicalDamage));
            if (MagicDamage != 0)
                param.AddParameterValue(paramSheet[MagicDamageKey], new ParameterValueFixed(ParameterType.Primary, MagicDamage));
            param.AddParameterValue(paramSheet[DelayKey], new ParameterValueFixed(ParameterType.Primary, Delay.TotalSeconds));

            return param;
        }
        #endregion
    }
}