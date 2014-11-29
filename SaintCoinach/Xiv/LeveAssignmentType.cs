using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class LeveAssignmentType : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public bool IsFaction { get { return AsBoolean("IsFaction"); } }
        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }
        #endregion

        #region Constructor
        public LeveAssignmentType(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}