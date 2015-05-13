using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class ChocoboRaceTerritory : XivRow {
        #region Properties

        public Text.XivString Name { get { return As<GoldSaucerTextData>("Name").Text; } }
        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }

        #endregion

        #region Constructors

        public ChocoboRaceTerritory(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
