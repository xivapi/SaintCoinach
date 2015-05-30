using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class HousingLayoutLimit : XivRow {
        #region Properties

        public int PersonalChamber { get { return AsInt32("PersonalChamber"); } }
        public int Cottage { get { return AsInt32("Small"); } }
        public int House { get { return AsInt32("Medium"); } }
        public int Mansion { get { return AsInt32("Large"); } }

        #endregion

        #region Constructors

        protected HousingLayoutLimit(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
