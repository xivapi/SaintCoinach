using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class MainCommand : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public string Description { get { return AsString("Description"); } }
        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }
        public MainCommandCategory MainCommandCategory { get { return As<MainCommandCategory>(); } }
        #endregion

        #region Constructor
        public MainCommand(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}