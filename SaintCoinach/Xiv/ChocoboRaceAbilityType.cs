using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class ChocoboRaceAbilityType : XivRow {
        #region Properties

        public bool IsActive { get { return AsBoolean("IsActive"); } }

        #endregion

        #region Constructors

        public ChocoboRaceAbilityType(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
