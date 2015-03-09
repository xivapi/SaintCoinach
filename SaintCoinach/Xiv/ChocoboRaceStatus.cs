using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class ChocoboRaceStatus : XivRow {
        #region Properties

        public Status Status { get { return As<Status>(); } }

        #endregion

        #region Constructors

        public ChocoboRaceStatus(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Status.Name;
        }
    }
}
