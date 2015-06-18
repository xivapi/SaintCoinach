using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class AirshipExplorationParamType : XivRow {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }

        #endregion

        #region Constructors

        public AirshipExplorationParamType(IXivSheet sheet, SaintCoinach.Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}