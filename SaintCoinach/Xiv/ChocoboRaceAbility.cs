using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class ChocoboRaceAbility : ActionBase {
        #region Properties

        public ChocoboRaceAbilityType Type { get { return As<ChocoboRaceAbilityType>(); } }
        public int Strength { get { return AsInt32("Value"); } }

        #endregion

        #region Constructors

        public ChocoboRaceAbility(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion


    }
}
