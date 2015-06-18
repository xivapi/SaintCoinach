using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class AirshipExplorationPoint  : XivRow {
        #region Properties

        public Text.XivString Name { get { return AsString("Name"); } }
        public Text.XivString ShortName { get { return AsString("Name{Short}"); } }

        public int RequiredLevel { get { return AsInt32("RequiredLevel"); } }

        public int RequiredFuel { get { return AsInt32("RequiredFuel"); } }

        public TimeSpan Duration { get { return TimeSpan.FromMinutes(AsInt32("Duration<min>")); } }

        public int ExpReward { get { return AsInt32("ExpReward"); } }

        #endregion

        #region Constructors

        public AirshipExplorationPoint(IXivSheet sheet, SaintCoinach.Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
