using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class Achievement : XivRow {
        #region Properties
        public AchievementCategory AchievementCategory { get { return As<AchievementCategory>(); } }
        public string Name { get { return AsString("Name"); } }
        public string Description { get { return AsString("Description"); } }
        public int Points { get { return AsInt32("Points"); } }
        public Title Title { get { return As<Title>(); } }
        public Item Item { get { return As<Item>("Item"); } }
        public Imaging.ImageFile Icon { get { return AsImage("Icon"); } }
        public int Order { get { return AsInt32("Order"); } }
        #endregion

        #region Constructor
        public Achievement(IXivSheet sheet, Ex.Relational.IRelationalRow sourceRow) : base(sheet, sourceRow) { }
        #endregion

        public override string ToString() {
            return Name;
        }
    }
}
