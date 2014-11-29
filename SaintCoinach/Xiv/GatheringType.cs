using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class GatheringType : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public Imaging.ImageFile MainIcon { get { return AsImage("Icon{Main}"); } }
        public Imaging.ImageFile SubIcon { get { return AsImage("Icon{Off}"); } }
        #endregion

        #region Constructor
        public GatheringType(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}