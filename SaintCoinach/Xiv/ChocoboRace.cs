using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach.Ex.Relational;

namespace SaintCoinach.Xiv {
    public class ChocoboRace : XivRow {
        #region Properties

        public ChocoboRaceRank Rank { get { return As<ChocoboRaceRank>(); } }
        public ChocoboRaceTerritory Territory { get { return As<ChocoboRaceTerritory>(); } }

        #endregion

        #region Constructors

        public ChocoboRace(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        public override string ToString() {
            return string.Format("{0}: {1}", Rank.Name, Territory.Name);
        }
    }
}
