using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class RetainerTask : XivRow, IItemSource {
        #region Properties

        public ClassJobCategory ClassJobCategory { get { return As<ClassJobCategory>(); } }
        public bool IsRandom { get { return AsBoolean("IsRandom"); } }
        public int Level { get { return AsInt32("Level"); } }
        public int VentureCost { get { return AsInt32("VentureCost"); } }
        public TimeSpan BaseDuration { get { return TimeSpan.FromMinutes(AsInt32("MaxTime{min}")); } }
        public int Experience { get { return AsInt32("Experience"); } }
        public int RequiredItemLevel { get { return AsInt32("RequiredItemLevel"); } }
        public int RequiredGathering { get { return AsInt32("RequiredGathering"); } }
        public RetainerTaskBase Task { get { return As<RetainerTaskBase>("Task"); } }

        public IEnumerable<Item> Items { get { return ((IItemSource)Task).Items; } }

        #endregion

        #region Constructors

        public RetainerTask(IXivSheet sheet, SaintCoinach.Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion
    }
}
