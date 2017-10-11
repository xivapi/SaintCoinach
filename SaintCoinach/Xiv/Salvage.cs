using SaintCoinach.Ex.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class Salvage : XivRow {
        #region Properties

        public int OptimalSkill { get { return AsInt32("OptimalSkill"); } }

        #endregion

        #region Constructors

        #region Constructor

        public Salvage(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return OptimalSkill.ToString();
        }
    }
}
