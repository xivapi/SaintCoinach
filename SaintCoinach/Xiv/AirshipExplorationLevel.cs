using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class AirshipExplorationLevel : XivRow {
        #region Properties

        public int ExpToNext { get { return AsInt32("ExpToNext"); } }

        #endregion

        #region Constructors

        public AirshipExplorationLevel(IXivSheet sheet, SaintCoinach.Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
