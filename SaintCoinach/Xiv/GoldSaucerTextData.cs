using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class GoldSaucerTextData : XivRow {
        #region Properties

        public Text.XivString Text { get { return AsString("Text"); } }

        #endregion

        #region Constructors

        public GoldSaucerTextData(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Text;
        }
    }
}
