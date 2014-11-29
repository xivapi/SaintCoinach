using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class Mount : XivRow {
        #region Properties
        public string Singular { get { return AsString("Singular"); } }
        public string Plural { get { return AsString("Plural"); } }
        public string Description { get { return AsString("Description"); } }
        public string GuideDescription { get { return AsString("Description{Enhanced}"); } }
        public string Tooltip { get { return AsString("Tooltip"); } }
        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }
        #endregion

        #region Constructor
        public Mount(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Singular;
        }
    }
}