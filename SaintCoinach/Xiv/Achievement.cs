using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class Achievement : XivRow, IItemSource {
        #region Properties

        public AchievementCategory AchievementCategory { get { return As<AchievementCategory>(); } }
        public Text.XivString Name { get { return AsString("Name"); } }
        public Text.XivString Description { get { return AsString("Description"); } }
        public int Points { get { return AsInt32("Points"); } }
        public Title Title { get { return As<Title>(); } }
        public Item Item { get { return As<Item>("Item"); } }
        public ImageFile Icon { get { return AsImage("Icon"); } }
        public int Order { get { return AsInt32("Order"); } }

        #endregion

        #region Constructors

        #region Constructor

        public Achievement(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion

        public override string ToString() {
            return Name;
        }

        #region IItemSource Members

        System.Collections.Generic.IEnumerable<Item> IItemSource.Items {
            get { yield return Item; }
        }

        #endregion
    }
}
