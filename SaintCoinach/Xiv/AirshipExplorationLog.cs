using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class AirshipExplorationLog : XivRow {
        #region Properties

        public Text.XivString Text { get { return AsString("Text"); } }

        #endregion

        #region Constructors

        public AirshipExplorationLog(IXivSheet sheet, SaintCoinach.Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Text;
        }
    }
}