using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class RacingChocoboNameInfo : XivRow {
        #region Properties

        public RacingChocoboNameCategory Category { get { return As<RacingChocoboNameCategory>(); } }

        #endregion

        #region Constructors

        public RacingChocoboNameInfo(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
