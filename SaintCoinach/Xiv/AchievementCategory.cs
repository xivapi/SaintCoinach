using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class AchievementCategory : XivRow {
        #region Properties
        public string Name { get { return AsString("Name"); } }
        public AchievementKind AchievementKind { get { return As<AchievementKind>(); } }
        #endregion

        #region Constructor
        public AchievementCategory(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
