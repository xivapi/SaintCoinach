using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class BeastReputationRank : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public int RequiredReputation { get { return AsInt32("RequiredReputation"); } }
        #endregion

        #region Constructor
        public BeastReputationRank(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}