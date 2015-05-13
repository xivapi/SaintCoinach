using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class LeveVfx : XivRow {
        #region Properties

        public Text.XivString Effect { get { return AsString("Effect"); } }

        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }

        #endregion

        #region Constructors

        public LeveVfx(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
