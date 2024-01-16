using SaintCoinach.Ex.Relational;
using SaintCoinach.Imaging;

namespace SaintCoinach.Xiv {
    public class GrandCompanyRank : XivRow {
        #region Properties

        public int Tier { get { return AsInt32("Tier"); } }
        public int Order { get { return AsInt32("Order"); } }
        public int MaximumSeals { get { return AsInt32("MaxSeals"); } }
        public int RequiredSeals { get { return AsInt32("RequiredSeals"); } }
        public ImageFile MaelstromIcon { get { return AsImage("IconMaelstrom"); } }
        public ImageFile SerpentsIcon { get { return AsImage("IconSerpents"); } }
        public ImageFile FlamesIcon { get { return AsImage("IconFlames"); } }
        public Quest MaelstromQuest { get { return As<Quest>("QuestMaelstrom"); } }
        public Quest SerpentsQuest { get { return As<Quest>("QuestSerpents"); } }
        public Quest FlamesQuest { get { return As<Quest>("QuestFlames"); } }

        #endregion

        #region Constructors

        #region Constructor

        public GrandCompanyRank(IXivSheet sheet, IRelationalRow sourceRow) : base(sheet, sourceRow) { }

        #endregion

        #endregion
    }
}
