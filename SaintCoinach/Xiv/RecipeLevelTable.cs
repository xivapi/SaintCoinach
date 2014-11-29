using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class RecipeLevelTable : XivRow {
        #region Properties
        public int Difficulty { get { return AsInt32("Difficulty"); } }
        public int Quality { get { return AsInt32("Quality"); } }
        public int Durability { get { return AsInt32("Durability"); } }
        #endregion

        #region Constructor
        public RecipeLevelTable(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion
    }
}