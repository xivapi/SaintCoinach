using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class ENpcResident : XivRow {
        #region Properties
        public string Singular { get { return AsString("Singular"); } }
        public string Plural { get { return AsString("Plural"); } }
        public string Title { get { return AsString("Title"); } }
        public Map Map { get { return As<Map>(); } }
        #endregion

        #region Constructor
        public ENpcResident(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Singular;
        }
    }
}