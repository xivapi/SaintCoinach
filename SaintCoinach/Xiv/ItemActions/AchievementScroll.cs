using SaintCoinach.Ex.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv.ItemActions {
    public class AchievementScroll : ItemAction {
        // Used to get a certain achievement, such as Saint of Firmament.

        const int AchievementKey = 0;

        public AchievementScroll(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        public Achievement Achievement {
            get {
                return Sheet.Collection.GetSheet<Achievement>()[GetData(AchievementKey)];
            }
        }
    }
}
