using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class Weather : XivRow {
        #region Properties
        public string Name { get { return (string)this["Name"]; } }
        public string Description { get { return (string)this["Description"]; } }
        #endregion

        #region ToString
        public override string ToString() {
            return Name;
        }
        #endregion

        #region Constructor
        public Weather(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion
    }
}
