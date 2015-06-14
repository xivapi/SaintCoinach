using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class ParamGrow : XivRow {
        #region Properties
        public int ExpToNext { get { return AsInt32("ExpToNext"); } }
        public int AdditionalActions { get { return AsInt32("AdditionalActions"); } }
        public double MpCostFactor { get { return AsInt32("MpCostFactor<%>") / 100.0; } }
        #endregion

        #region Constructors

        public ParamGrow(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
