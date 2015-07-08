using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class RetainerTaskRandom : RetainerTaskBase {
        #region Properties
        
        public Text.XivString Name { get { return AsString("Name"); } }
        public int Requirement { get { return AsInt32("Requirement"); } }
        public override IEnumerable<Item> Items { get { yield break; } }

        #endregion

        #region Constructors

        public RetainerTaskRandom(IXivSheet sheet, SaintCoinach.Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
