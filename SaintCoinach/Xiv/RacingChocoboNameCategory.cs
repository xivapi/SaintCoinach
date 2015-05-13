using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class RacingChocoboNameCategory : XivRow {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }

        public int SortKey { get { return AsInt32("SortKey"); } }

        #endregion

        #region Constructors

        public RacingChocoboNameCategory(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
