using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class ChocoboRaceRank : XivRow {
        #region Properties

        public Text.XivString Name { get { return As<GoldSaucerTextData>("Name").Text; } }
        public int MinimumRating { get { return AsInt32("Rating{Min}"); } }
        public int MaximumRating { get { return AsInt32("Rating{Max}"); } }
        public int Fee { get { return AsInt32("Fee"); } }
        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }

        #endregion

        #region Constructors

        public ChocoboRaceRank(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
