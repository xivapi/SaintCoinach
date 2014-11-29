using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class PlaceName : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public string NameWithoutArticle { get { return AsString("Name{NoArticle}"); } }
        #endregion

        #region Constructor
        public PlaceName(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}