using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class TripleTriadCardType : XivRow {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }

        #endregion

        #region Constructors

        public TripleTriadCardType(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
